namespace ProCenter.Service.Message.Patient
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class GetPatientDtoByKeyRequest : Request
    {
        public Guid PatientKey { get; set; }
    }
}