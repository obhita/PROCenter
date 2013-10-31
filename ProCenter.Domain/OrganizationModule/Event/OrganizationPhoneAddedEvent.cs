namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Organization phone added event.
    /// </summary>
    public class OrganizationPhoneAddedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationPhoneAddedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationPhone">The organization phone.</param>
        public OrganizationPhoneAddedEvent ( Guid key, int version, OrganizationPhone organizationPhone )
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