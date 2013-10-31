namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class SubmitAssessmentRequest : Request
    {
        public Guid AssessmentKey { get; set; }
        public bool Submit { get; set; }
    }
}