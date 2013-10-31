namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Assessment scored event.
    /// </summary>
    public class AssessmentScoredEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentScoredEvent" /> class.
        /// </summary>
        /// <param name="assessmentKey">The assessment key.</param>
        /// <param name="version">The version.</param>
        /// <param name="scoreCode">The score code.</param>
        /// <param name="value">The value.</param>
        /// <param name="guidance">The guidance.</param>
        public AssessmentScoredEvent ( Guid assessmentKey, int version, CodedConcept scoreCode, object value, CodedConcept guidance = null )
            : base ( assessmentKey, version )
        {
            Value = value;
            Guidance = guidance;
            ScoreCode = scoreCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the guidance.
        /// </summary>
        /// <value>
        ///     The guidance.
        /// </value>
        public CodedConcept Guidance { get; private set; }

        /// <summary>
        ///     Gets the score code.
        /// </summary>
        /// <value>
        ///     The score code.
        /// </value>
        public CodedConcept ScoreCode { get; private set; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value { get; private set; }

        #endregion
    }
}