namespace ProCenter.Domain.SecurityModule.Event
{
    using System;
    using CommonModule;

    public class SystemAccountUnLockedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountUnLockedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        public SystemAccountUnLockedEvent(Guid key, int version)
            : base(key, version)
        {
        }

        #endregion
    }
}