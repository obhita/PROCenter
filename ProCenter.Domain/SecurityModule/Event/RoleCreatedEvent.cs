namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Role Created Event.
    /// </summary>
    public class RoleCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="roleType">Type of the role.</param>
        public RoleCreatedEvent(Guid key, int version, string name, RoleType roleType)
            : base(key, version)
        {
            Name = name;
            RoleType = roleType;
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
        /// Gets the type of the role.
        /// </summary>
        /// <value>
        /// The type of the role.
        /// </value>
        public RoleType RoleType { get; private set; }

        #endregion
    }
}