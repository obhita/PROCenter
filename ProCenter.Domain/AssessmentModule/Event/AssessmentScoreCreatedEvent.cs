namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Assessment score created event.
    /// </summary>
    public class AssessmentScoreCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentScoreCreatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentKey">The assessment key.</param>
        /// <param name="version">The version.</param>
        public AssessmentScoreCreatedEvent ( Guid assessmentKey, int version )
            : base ( assessmentKey, version )
        {
        }

        #endregion
    }
}