namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event for removal of organization phone.
    /// </summary>
    public class OrganizationPhoneRemovedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationPhoneRemovedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationPhone">The organization phone.</param>
        public OrganizationPhoneRemovedEvent ( Guid key, int version, OrganizationPhone organizationPhone )
            : base ( key, version )
        {
            OrganizationPhone = organizationPhone;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the organization phone.
        /// </summary>
        /// <value>
        ///     The organization phone.
        /// </value>
        public OrganizationPhone OrganizationPhone { get; private set; }

        #endregion
    }
}