namespace ProCenter.Domain.SecurityModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;
    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>
    ///     Role aggregate root.
    /// </summary>
    public class Role : AggregateRootBase
    {
        #region Fields

        private readonly List<Permission> _permissions = new List<Permission>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="roleType">Type of the role.</param>
        public Role(string name, RoleType roleType = RoleType.UserDefined)
        {
            Check.IsNotNullOrWhitespace(name, () => Name);

            Key = CombGuid.NewCombGuid();
            RaiseEvent(new RoleCreatedEvent(Key, Version, name, roleType));
        }

        public Role()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid? OrganizationKey { get; private set; }

        /// <summary>
        ///     Gets the permissions.
        /// </summary>
        /// <value>
        ///     The permissions.
        /// </value>
        public IEnumerable<Permission> Permissions
        {
            get { return _permissions; }
        }

        /// <summary>
        /// Gets the type of the role.
        /// </summary>
        /// <value>
        /// The type of the role.
        /// </value>
        public RoleType RoleType { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void ReviseName(string name)
        {
            RaiseEvent(new RoleNameRevisedEvent(Key, Version, name));
        }

        /// <summary>
        ///     Adds the permision.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void AddPermision(Permission permission)
        {
            RaiseEvent(new PermissionAddedEvent(Key, Version, permission));
        }

        /// <summary>
        ///     Removes the permision.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void RemovePermision(Permission permission)
        {
            RaiseEvent(new PermissionRemovedEvent(Key, Version, permission));
        }

        #endregion

        #region Methods

        private void Apply(RoleNameRevisedEvent roleNameRevisedEvent)
        {
            Name = roleNameRevisedEvent.Name;
        }

        private void Apply(PermissionAddedEvent permissionAddedEvent)
        {
            _permissions.Add(permissionAddedEvent.Permission);
        }

        private void Apply(PermissionRemovedEvent permissionRemovedEvent)
        {
            _permissions.Remove(permissionRemovedEvent.Permission);
        }

        private void Apply(RoleCreatedEvent roleCreatedEvent)
        {
            OrganizationKey = roleCreatedEvent.OrganizationKey;
            Name = roleCreatedEvent.Name;
            RoleType = roleCreatedEvent.RoleType;
        }

        #endregion
    }
}