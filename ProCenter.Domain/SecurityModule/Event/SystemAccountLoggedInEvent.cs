namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     System account logged in event.
    /// </summary>
    public class SystemAccountLoggedInEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountLoggedInEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="time">The time.</param>
        public SystemAccountLoggedInEvent ( Guid key, int version, DateTime time )
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