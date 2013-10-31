namespace ProCenter.Domain.SecurityModule.Event
{
    using System;
    using CommonModule;

    public class SystemAccountRoleRemovedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountRoleRemovedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="roleKey">The role key.</param>
        public SystemAccountRoleRemovedEvent(Guid key, int version, Guid roleKey)
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