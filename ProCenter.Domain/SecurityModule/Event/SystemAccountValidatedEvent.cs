namespace ProCenter.Domain.SecurityModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     System account validated event.
    /// </summary>
    public class SystemAccountValidatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountValidatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="validated">
        ///     if set to <c>true</c> [validated].
        /// </param>
        public SystemAccountValidatedEvent ( Guid key, int version, bool validated )
            : base ( key, version )
        {
            Validated = validated;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this <see cref="SystemAccountValidatedEvent" /> is validated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if validated; otherwise, <c>false</c>.
        /// </value>
        public bool Validated { get; private set; }

        #endregion
    }
}