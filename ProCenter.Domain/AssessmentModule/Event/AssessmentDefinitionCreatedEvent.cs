namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event for when an assessment definition is created.
    /// </summary>
    public class AssessmentDefinitionCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentDefinitionCreatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="version">The version.</param>
        /// <param name="codedConcept">The coded concept.</param>
        public AssessmentDefinitionCreatedEvent(Guid assessmentDefinitionKey, int version, CodedConcept codedConcept)
            : base ( assessmentDefinitionKey, version )
        {
            CodedConcept = codedConcept;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; private set; }

        #endregion
    }
}