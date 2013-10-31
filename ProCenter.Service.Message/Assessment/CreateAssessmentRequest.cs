namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class CreateAssessmentRequest : Request
    {
        public Guid PatientKey { get; set; }

        public Guid AssessmentDefinitionKey { get; set; }

        public Guid? WorkflowKey { get; set; }

        public bool ForSelfAdministration { get; set; }
    }
}