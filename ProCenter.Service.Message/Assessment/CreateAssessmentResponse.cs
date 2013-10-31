namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class CreateAssessmentResponse : Response
    {
        public Guid AssessmentInstanceKey { get; set; }
    }
}