namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Assessment Submitted event.
    /// </summary>
    public class AssessmentSubmittedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentSubmittedEvent" /> class.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        /// <param name="version">The version.</param>
        /// <param name="submit">
        ///     if set to <c>true</c> [submit].
        /// </param>
        public AssessmentSubmittedEvent ( Guid assessmentInstanceKey, int version, bool submit )
            : base ( assessmentInstanceKey, version )
        {
            Submit = submit;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AssessmentSubmittedEvent" /> is submit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if submit; otherwise, <c>false</c>.
        /// </value>
        public bool Submit { get; set; }

        #endregion
    }
}