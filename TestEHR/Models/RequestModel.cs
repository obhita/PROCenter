namespace TestEHR.Models
{
    #region Using Statements

    using System;

    #endregion

    public class RequestModel
    {
        public string Url { get; set; }
        public string EhrId { get; set; }
        public Guid PatientId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid? AssessmentId { get; set; }
        public string Timestamp { get; set; }
        public string ReturnUrl { get; set; }
        public string Token { get; set; }
    }
}