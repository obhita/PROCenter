namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event when assessment is created.
    /// </summary>
    public class AssessmentCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentCreatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        /// <param name="version">The version.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="assessmentName">The assessment name.</param>
        public AssessmentCreatedEvent(Guid assessmentInstanceKey, int version, Guid patientKey, Guid assessmentDefinitionKey, string assessmentName)
            : base ( assessmentInstanceKey, version )
        {
            PatientKey = patientKey;
            AssessmentDefinitionKey = assessmentDefinitionKey;
            AssessmentName = assessmentName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the assessment definition key.
        /// </summary>
        /// <value>
        ///     The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; private set; }

        /// <summary>
        /// Gets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; private set; }

        /// <summary>
        ///     Gets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        #endregion
    }
}