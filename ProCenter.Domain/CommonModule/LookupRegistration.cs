namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Lookups;

    #endregion

    /// <summary>
    ///     Attribute for defining registration lookup type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LookupRegistration : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LookupRegistration" /> class.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <exception cref="System.InvalidOperationException">Cannot use type that does not inherit from type Lookup.</exception>
        public LookupRegistration ( Type lookupType )
        {
            if ( !typeof(Lookup).IsAssignableFrom ( lookupType ) )
            {
                throw new InvalidOperationException ( string.Format ( "Cannot use type {0} because it does not inherit from type Lookup.", lookupType ) );
            }
            LookupType = lookupType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the type of the lookup.
        /// </summary>
        /// <value>
        ///     The type of the lookup.
        /// </value>
        public Type LookupType { get; private set; }

        #endregion
    }
}