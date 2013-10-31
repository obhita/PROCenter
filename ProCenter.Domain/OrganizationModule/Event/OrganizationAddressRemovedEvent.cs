namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event for removal of organization address.
    /// </summary>
    public class OrganizationAddressRemovedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationAddressRemovedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationAddress">The organization address.</param>
        public OrganizationAddressRemovedEvent ( Guid key, int version, OrganizationAddress organizationAddress )
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