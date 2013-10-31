namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Primitive;

    #endregion

    /// <summary>
    ///     Staff Created Event.
    /// </summary>
    public class StaffCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaffCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="name">The name.</param>
        public StaffCreatedEvent ( Guid key, int version, Guid organizationKey, PersonName name )
            : base ( key, version )
        {
            OrganizationKey = organizationKey;
            Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public PersonName Name { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid OrganizationKey { get; private set; }

        #endregion
    }
}