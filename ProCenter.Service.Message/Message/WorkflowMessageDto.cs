namespace ProCenter.Service.Message.Message
{
    #region Using Statements

    using System;
    using Assessment;
    using Common;

    #endregion

    public class WorkflowMessageDto : KeyedDataTransferObject, IMessageDto
    {
        protected bool Equals(WorkflowMessageDto other)
        {
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorkflowMessageDto) obj);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public Guid PatientKey { get; set; }
        public ScoreDto InitiatingAssessmentScore { get; set; }
        public Guid InitiatingAssessmentKey { get; set; }
        public string InitiatingAssessmentCode { get; set; }
        public Guid RecommendedAssessmentDefinitionKey { get; set; }
        public string RecommendedAssessmentDefinitionCode { get; set; }
    }
}