namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Organization address added event.
    /// </summary>
    public class OrganizationAddressAddedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationAddressAddedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationAddress">The organization address.</param>
        public OrganizationAddressAddedEvent ( Guid key, int version, OrganizationAddress organizationAddress )
            : base ( key, version )
        {
            OrganizationAddress = organizationAddress;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the organization address.
        /// </summary>
        /// <value>
        ///     The organization address.
        /// </value>
        public OrganizationAddress OrganizationAddress { get; private set; }

        #endregion
    }
}