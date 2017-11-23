using System.Net.Http.Formatting;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;
using System.Web.Routing;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Controllers;
using CheckINN.WebApi.Formatters;
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
        private readonly IDependencyResolver _resolver;

        public ApiHost()
        {
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
            var container = new UnityContainer();
            container.RegisterInstance("tessdata-location", @"tessdata\", new ContainerControlledLifetimeManager());
            container.RegisterInstance("tess-language", "lit", new ContainerControlledLifetimeManager());
            container.RegisterType<ITextRecognition, TesseractTextRecognition>(
                new InjectionConstructor(
                    new ResolvedParameter<string>("tessdata-location"), 
                    new ResolvedParameter<string>("tess-language")));
            container.RegisterType<ICheckCache, CheckCache>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICheckProcessor, BasicCheckProcessor>();
            container.RegisterType<IShopParser, SimpleShopParser>();
            container.RegisterType<ILog>(new InjectionFactory(log => ResolveLogger()));
            RegisterControllers(ref container);
            return container;
        }

        /// <summary>
        /// Register ASP.NET controllers
        /// </summary>
        /// <param name="container">Reference to container that is used for everything</param>
        private void RegisterControllers(ref UnityContainer container)
        {
            container.RegisterType<IHttpController, StatusController>("status");
            container.RegisterType<IHttpController, ReceiptController>("receipt");
        }

        /// <summary>
        /// Creates HTTP server configuration and runs it
        /// </summary>
        /// TODO: Should separate HTTP server from any backing cache services.
        /// TODO: That would allow stopping HTTP server before the backend,
        /// TODO: allowing any async processes to finish work gracefully
        /// TODO: without being fed new data continiously (that's called draining)
        public void Start()
        {
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
            _server.OpenAsync().Wait();
        }


        /// <summary>
        /// Sends a signal to shut the service down
        /// </summary>
        /// TODO: Cancellation tokens should be used for this is the future, to cause graceful shutdown
        public void Stop()
        {
            _server.Dispose();
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
