using System.ServiceModel;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.Domain.Image;
using CheckINN.WebApi.Controllers;
using CheckINN.WebApi.Formatters;
using CheckINN.WebApi.Workers;
using log4net;
using log4net.Config;
using Topshelf;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace CheckINN.WebApi
{
    /// <summary>
    /// Describes and runs the entire service
    /// </summary>
    public class ApiHost
    {
        private HttpSelfHostServer _server;
        private ImageWorker _imageWorker;
        private readonly IDependencyResolver _resolver;
        private IUnityContainer _container;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public ApiHost()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var container = BuildContainer();
            _resolver = new UnityDependencyResolver(container, ResolveLogger());
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        /// <summary>
        /// Resolved logger instances
        /// </summary>
        /// <returns>A logger instance</returns>
        private ILog ResolveLogger()
        {
            return LogManager.GetLogger("CheckINN.WebApi");
        }

        private IUnityContainer BuildContainer()
        {
            _container = new UnityContainer();
            _container.RegisterInstance("tessdata-location", @"tessdata\", new ContainerControlledLifetimeManager());
            _container.RegisterInstance("tess-language", "lit", new ContainerControlledLifetimeManager());
            _container.RegisterInstance("tess-mode", 0, new ContainerControlledLifetimeManager());
            _container.RegisterType<ITextRecognition, TesseractTextRecognition>(
                new InjectionConstructor(
                    new ResolvedParameter<string>("tessdata-location"), 
                    new ResolvedParameter<string>("tess-language"),
                    new ResolvedParameter<int>("tess-mode")));
            _container.RegisterType<ICheckCache, CheckCache>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBitmapQueueCache, BitmapQueueCache>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICheckProcessor, BasicCheckProcessor>();
            _container.RegisterType<IShopParser, SimpleShopParser>();
            _container.RegisterType<ILog>(
                new InjectionFactory(
                    delegate(IUnityContainer container)
            {
                return LogManager.GetLogger("CheckINN.WebApi");
            }));
            _container.RegisterInstance(_cancellationTokenSource.Token);
            _container.RegisterType<ITransform, Transformator>();
            _container.RegisterType<ImageWorker>(new ContainerControlledLifetimeManager());
            RegisterControllers(ref _container);
            return _container;
        }

        /// <summary>
        /// Register ASP.NET controllers
        /// </summary>
        /// <param name="container">Reference to container that is used for everything</param>
        private void RegisterControllers(ref IUnityContainer container)
        {
            container.RegisterType<IHttpController, StatusController>("status");
            container.RegisterType<IHttpController, ReceiptController>("receipt");
            container.RegisterType<IHttpController, CacheController>("cache");
            container.RegisterType<IHttpController, NotificationController>("notification", new PerResolveLifetimeManager());
        }

        /// <summary>
        /// Creates HTTP server configuration and runs it
        /// </summary>
        public void Start()
        {
            _imageWorker = _container.Resolve<ImageWorker>();
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            config.DependencyResolver = _resolver;
            config.Formatters.Add(new SingleBitmapFormatter(ResolveLogger()));
            config.Routes.MapHttpRoute("API Default", "api/{controller}");
            config.Routes.MapHttpRoute("Receipt API", "api/receipt/{action}", new {controller = "Receipt", action = "PostReceipt" });
            config.Routes.MapHttpRoute("Cache API", "api/cache", new { controller = "Cache" });
            config.MaxBufferSize = 50 * 1024 * 1024;
            config.MaxReceivedMessageSize = 50 * 1024 * 1024;
            config.TransferMode = TransferMode.StreamedRequest;

            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait(_cancellationTokenSource.Token);
        }


        /// <summary>
        /// Sends a signal to shut the service down
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }

    /// <summary>
    /// Process starting point
    /// </summary>
    class Program
    {

        /// <summary>
        /// Sets up Topshelf as a service managing tool
        /// </summary>
        /// <param name="args">it's pretty obvious what this does, and it's unused by the application</param>
        static void Main(string[] args)
        {
            BasicConfigurator.Configure();
            HostFactory.Run(configurator =>
            {
                configurator.Service<ApiHost>(service =>
                {
                    service.ConstructUsing(() => new ApiHost());
                    service.WhenStarted(host => host.Start());
                    service.WhenStopped(host => host.Stop());
                });
                configurator.RunAsLocalSystem();

                configurator.SetDescription("Backend processing api for CheckINN application");
                configurator.SetDisplayName("CheckINN.WebApi");
                configurator.SetServiceName("CheckINN.WebApi");
            });
        }
    }
}
