namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>
    ///     Permission Removed event.
    /// </summary>
    public class PermissionRemovedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PermissionRemovedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="permission">The permission.</param>
        public PermissionRemovedEvent(Guid key, int version, Permission permission)
            : base(key, version)
        {
            Permission = permission;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the permission.
        /// </summary>
        /// <value>
        ///     The permission.
        /// </value>
        public Permission Permission { get; private set; }

        #endregion
    }
}