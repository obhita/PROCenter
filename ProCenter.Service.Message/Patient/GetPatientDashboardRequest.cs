namespace ProCenter.Service.Message.Patient
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class GetPatientDashboardRequest : Request
    {
        public Guid PatientKey { get; set; }
    }
}