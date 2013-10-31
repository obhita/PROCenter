namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Domain.Primitives;

    #endregion

    /// <summary>
    ///     Event for when assessment reminder time is revised.
    /// </summary>
    public class AssessmentReminderRevisedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentReminderRevisedEvent" /> class.
        /// </summary>
        public AssessmentReminderRevisedEvent ()
            : base ( Guid.Empty, -1 )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentReminderRevisedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="time">The time.</param>
        /// <param name="unit">The unit.</param>
        public AssessmentReminderRevisedEvent ( Guid key, int version, double time, AssessmentReminderUnit unit, Email sendToEmail = null )
            : base ( key, version )
        {
            Time = time;
            Unit = unit;
            SendToEmail = sendToEmail;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the time.
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public double Time { get; private set; }

        /// <summary>
        ///     Gets the unit.
        /// </summary>
        /// <value>
        ///     The unit.
        /// </value>
        public AssessmentReminderUnit Unit { get; private set; }

        /// <summary>
        /// Gets the send to email.
        /// </summary>
        /// <value>
        /// The send to email.
        /// </value>
        public Email SendToEmail { get; private set; }

        #endregion
    }
}