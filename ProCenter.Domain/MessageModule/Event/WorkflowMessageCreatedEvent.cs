namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using AssessmentModule;

    #endregion

    public class WorkflowMessageCreatedEvent : MessageEventBase
    {
        public WorkflowMessageCreatedEvent(Guid key,
                                           MessageType messageType,
                                           Guid patientKey,
                                           WorkflowMessageStatus workflowMessageStatus,
                                           Guid initiatingAssessmentKey,
                                           string initiatingAssessmentCode,
                                           Guid recommendedAssessmentDefinitionKey,
                                           string recommendedAssessmentDefinitionCode,
                                           Score initiatingAssessmentScore)
            : base(key, messageType)
        {
            PatientKey = patientKey;
            WorkflowMessageStatus = workflowMessageStatus;
            InitiatingAssessmentKey = initiatingAssessmentKey;
            InitiatingAssessmentCode = initiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = recommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = recommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = initiatingAssessmentScore;
        }

        public Guid PatientKey { get; private set; }
        public WorkflowMessageStatus WorkflowMessageStatus { get; private set; }
        public Guid InitiatingAssessmentKey { get; private set; }
        public string InitiatingAssessmentCode { get; private set; }
        public Guid RecommendedAssessmentDefinitionKey { get; private set; }
        public string RecommendedAssessmentDefinitionCode { get; private set; }
        public Score InitiatingAssessmentScore { get; private set; }
    }
}