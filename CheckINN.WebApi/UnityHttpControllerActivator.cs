using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Unity;

namespace CheckINN.WebApi
{
    public class UnityHttpControllerActivator : IHttpControllerActivator
    {
        private readonly IUnityContainer _container;

        public UnityHttpControllerActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController) _container.Resolve(controllerType);
        }
    }
}