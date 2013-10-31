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
namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Reflection;
    using System.Resources;
    using Common;
    using Pillar.Common.InversionOfControl;

    #endregion

    /// <summary>
    ///     Manages resources
    /// </summary>
    public class ResourcesManager : IResourcesManager
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourcesManager" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ResourcesManager ( IContainer container )
        {
            _container = container;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the resource manager by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     A <see cref="ResourceManager" />.
        /// </returns>
        public ResourceManager GetResourceManagerByName ( string name )
        {
            return _container.Resolve<ResourceManager> ( name );
        }

        public void Register<TResource> (string code = null)
        {
            Register ( typeof(TResource), code );
        }

        public void Register(Type resourceType, string code = null)
        {
            Register ( resourceType.Name, resourceType.FullName, resourceType.Assembly, code );
        }

        public void Register(string name, string fullName, Assembly assembly, string code = null)
        {
            _container.RegisterInstance(typeof(ResourceManager), new ResourceManager(fullName, assembly), name);
            if (!string.IsNullOrEmpty(code))
            {
                _container.RegisterInstance(typeof (ResourceManager), new ResourceManager(fullName, assembly), code);
            }
        }

        #endregion
    }
}