namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Common;

    #endregion

    /// <summary>
    ///     Base class for commit events.
    /// </summary>
    public abstract class CommitEventBase : ICommitEvent
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommitEventBase" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        protected CommitEventBase ( Guid key, int version )
        {
            Key = key;
            Version = version;
            OrganizationKey = UserContext.Current.OrganizationKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the key.
        /// </summary>
        /// <value>
        ///     The key.
        /// </value>
        public Guid Key { get; private set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public Guid? OrganizationKey { get; private set; }

        #endregion
    }
}