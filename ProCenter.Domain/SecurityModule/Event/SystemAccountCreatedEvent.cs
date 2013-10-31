namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Domain.Primitives;

    #endregion

    /// <summary>
    ///     System Account Created Event.
    /// </summary>
    public class SystemAccountCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="email">The email.</param>
        public SystemAccountCreatedEvent(Guid key, int version, Guid organizationKey, string identifier, Email email)
            : base(key, version)
        {
            OrganizationKey = organizationKey;
            Identifier = identifier;
            Email = email;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public Email Email { get; private set; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Identifier { get; private set; }

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