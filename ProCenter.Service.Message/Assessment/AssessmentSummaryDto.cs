namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Common;

    #endregion

    public class AssessmentSummaryDto : KeyedDataTransferObject
    {
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public string CreatedTimeString
        {
            get { return CreatedTime.ToShortDateString(); }
        }

        public Guid PatientKey { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        public Guid AssessmentInstanceKey { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentCode { get; set; }

        public double PercentComplete { get; set; }
        public bool IsSubmitted { get; set; }
    }
}