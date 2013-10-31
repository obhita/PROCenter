namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     System account role added event.
    /// </summary>
    public class SystemAccountRoleAddedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountRoleAddedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="roleKey">The role key.</param>
        public SystemAccountRoleAddedEvent(Guid key, int version, Guid roleKey)
            : base(key, version)
        {
            RoleKey = roleKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the role key.
        /// </summary>
        /// <value>
        ///     The role key.
        /// </value>
        public Guid RoleKey { get; private set; }

        #endregion
    }
}