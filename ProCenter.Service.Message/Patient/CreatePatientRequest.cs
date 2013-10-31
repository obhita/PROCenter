namespace ProCenter.Service.Message.Patient
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    public class CreatePatientRequest : Request
    {
        public PatientDto PatientDto { get; set; }
    }
}