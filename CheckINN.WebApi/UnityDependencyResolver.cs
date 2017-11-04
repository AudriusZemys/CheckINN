using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using log4net;
using Unity;
using Unity.Exceptions;

namespace CheckINN.WebApi
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;
        private readonly ILog _log;

        public UnityDependencyResolver(IUnityContainer container, ILog log)
        {
            _container = container;
            _log = log;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                _log.Fatal($"Failed to resolve {serviceType.AssemblyQualifiedName}");
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                _log.Fatal($"Failed to resolve {serviceType.AssemblyQualifiedName}");
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new UnityDependencyResolver(child, _log);
        }
    }
}
