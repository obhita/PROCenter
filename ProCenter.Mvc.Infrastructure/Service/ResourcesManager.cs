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