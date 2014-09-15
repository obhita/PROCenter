#region License Header

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
    using System.Collections.Generic;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Caching;
    using Common;
    using NLog;

    #endregion

    /// <summary>Manages resources.</summary>
    public class ResourcesManager : IResourcesManager, IDisposable
    {
        #region Fields

        private readonly Logger _logger = LogManager.GetCurrentClassLogger ();
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy ();
        private readonly string _cachePrefix = typeof(ResourceManager).FullName + ".";
        private readonly Dictionary<string, Func<ResourceManager>> _resourceManagerMap = new Dictionary<string, Func<ResourceManager>> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourcesManager" /> class.
        /// </summary>
        public ResourcesManager ()
        {
            _cacheItemPolicy.RemovedCallback += arguments =>
            {
                if ( arguments.CacheItem.Value is IDisposable )
                {
                    ( arguments.CacheItem.Value as IDisposable ).Dispose ();
                }
            };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        /// <summary>
        ///     Gets the resource manager by name.
        /// </summary>
        /// <param name="name">The name of the resource manager.</param>
        /// <returns>
        ///     A <see cref="ResourceManager" />.
        /// </returns>
        public ResourceManager GetResourceManagerByName ( string name )
        {
            var cacheName = _cachePrefix + name;
            if ( !_resourceManagerMap.ContainsKey ( name ) )
            {
                return EmptyResources.ResourceManager;
            }
            if ( !_cache.Contains ( cacheName ) )
            {
                _cache.Add ( new CacheItem ( cacheName, _resourceManagerMap[name] () ), _cacheItemPolicy );
            }
            return _cache[cacheName] as ResourceManager;
        }

        /// <summary>Registers the specified code.</summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="code">The code.</param>
        public void Register<TResource> ( string code = null )
        {
            Register ( typeof(TResource), code );
        }

        /// <summary>Registers a resource manager for the type.</summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="code">The code.</param>
        public void Register ( Type resourceType, string code = null )
        {
            Register ( resourceType.Name, resourceType.FullName, resourceType.Assembly, code );
        }

        /// <summary>Registers a resource manager.</summary>
        /// <param name="name">The name.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="code">The code.</param>
        public void Register ( string name, string fullName, Assembly assembly, string code = null )
        {
            _logger.Debug ( "Resource Registered: {0} - {1} - {2} - {3}", name, fullName, assembly.FullName, code );
            _resourceManagerMap.Add ( name, () => new ResourceManager ( fullName, assembly ) );
            if ( !string.IsNullOrEmpty ( code ) )
            {
                _resourceManagerMap.Add ( code, () => new ResourceManager ( fullName, assembly ) );
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                _resourceManagerMap.Clear ();
                _cache.Dispose ();
            }
        }

        #endregion
    }
}