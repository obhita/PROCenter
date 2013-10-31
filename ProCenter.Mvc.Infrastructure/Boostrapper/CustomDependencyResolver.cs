#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Pillar.Common.InversionOfControl;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

#endregion

namespace ProCenter.Mvc.Infrastructure.Boostrapper
{
    public class CustomDependencyResolver : IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IContainer _container;

        public CustomDependencyResolver(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Gets the service.
        ///     Should return null when the service cannot be found
        ///     <see
        ///         cref="http://msdn.microsoft.com/en-us/library/system.web.mvc.idependencyresolver.getservice.aspx" />
        ///     .
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>An instance for the service.</returns>
        public object GetService(Type serviceType)
        {
            return _container.TryResolve(serviceType);
        }

        /// <summary>
        ///     Gets the services.
        ///     Should return an empty collection when no service can be found
        ///     <see
        ///         cref="http://msdn.microsoft.com/en-us/library/system.web.mvc.idependencyresolver.getservices.aspx" />
        ///     .
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>An IEnumberable of instances.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            var list = _container.ResolveAll(serviceType);
            var listObject = list.Cast<object>().ToList();
            return listObject;
        }

        /// <summary>
        ///     Starts a resolution scope.
        /// </summary>
        /// <returns>
        ///     The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return this;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }
    }
}