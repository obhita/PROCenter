namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    public class SystemAccountLockedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountLockedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="time">The time.</param>
        public SystemAccountLockedEvent ( Guid key, int version, DateTime time )
            : base ( key, version )
        {
            Time = time;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the time.
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public DateTime Time { get; private set; }

        #endregion
    }
}