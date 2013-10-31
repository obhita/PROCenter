namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Lookups;

    #endregion

    /// <summary>
    ///     Interface for lookup provider
    /// </summary>
    public interface ILookupProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Finds the specified code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        T Find<T> ( string code ) where T : Lookup;

        /// <summary>
        ///     Finds the specified lookup type.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Lookup Find ( string lookupType, string code );

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T> () where T : Lookup;

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <returns></returns>
        IEnumerable<Lookup> GetAll ( string lookupType );

        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Register<T> () where T : Lookup;

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <typeparam name="T">The actual type of the lookup.</typeparam>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        void Register<T> ( Type registerType ) where T : Lookup;

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="actualType">The actual type to pull lookups from.</param>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException">Cannot register a type that does not inherit from lookup type.</exception>
        void Register ( Type actualType, Type registerType );

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="lookupType">The lookup type.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        void Register ( Type lookupType );

        #endregion
    }
}