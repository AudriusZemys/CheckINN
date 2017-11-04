using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Metadata;
using System.Web.Http.SelfHost;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Parser;
using CheckINN.Domain.Processing;
using CheckINN.Domain.Services;
using CheckINN.WebApi.Controllers;
using log4net;
using Topshelf;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace CheckINN.WebApi
{
    public class ApiHost
    {
        private HttpSelfHostServer _server;
        private readonly IUnityContainer _container;

        public ApiHost()
        {
            _container = BuildContainer();
        }

        private ILog ResolveLogger()
        {
            return LogManager.GetLogger("CheckINN.WebApi");
        }

        private void RegisterControllers(ref UnityContainer container)
        {
            container.RegisterInstance<IHttpControllerActivator>(new UnityHttpControllerActivator(container));
            container.RegisterType<StatusController>();
        }

        private IUnityContainer BuildContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance(container);
            container.RegisterType<ITextRecognition, TesseractTextRecognition>();
            container.RegisterType<ICheckCache, CheckCache>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICheckProcessor, BasicCheckProcessor>();
            container.RegisterType<IShopParser, SimpleShopParser>();
            container.RegisterType<ILog>(new InjectionFactory(log => ResolveLogger()));
            RegisterControllers(ref container);
            return container;
        }

        public void Start()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080")
            {
                DependencyResolver = new UnityDependencyResolver(_container, ResolveLogger())
                
            };

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}",
                new { id = RouteParameter.Optional });

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
