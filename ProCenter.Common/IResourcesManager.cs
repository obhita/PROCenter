namespace ProCenter.Common
{
    #region Using Statements

    using System;
    using System.Reflection;
    using System.Resources;

    #endregion

    /// <summary>
    ///     Interface for managing resources.
    /// </summary>
    public interface IResourcesManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the resource manager by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     A <see cref="ResourceManager" />.
        /// </returns>
        ResourceManager GetResourceManagerByName ( string name );

        /// <summary>
        /// Registers a resource manager for the type.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        void Register<TResource> (string code = null);

        /// <summary>
        /// Registers a resource manager for the type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        void Register(Type resourceType, string code = null);

        /// <summary>
        /// Registers a resource manager.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="assembly">The assembly.</param>
        void Register(string name, string fullName, Assembly assembly, string code = null);

        #endregion
    }
}