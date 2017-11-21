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

        private void RegisterControllers(ref UnityContainer container)
        {
            container.RegisterType<IHttpController, StatusController>("status");
            container.RegisterType<IHttpController, ReceiptController>("receipt");
        }

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

        public void Stop()
        {
            _server.Dispose();
        }
    }
    class Program
    {
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
