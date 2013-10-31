#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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