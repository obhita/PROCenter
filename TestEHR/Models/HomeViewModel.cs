namespace TestEHR.Models
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    public class HomeViewModel
    {
        public IEnumerable<PatientViewModel> Patients { get; set; }
        public string ErrorMessage { get; set; }

        public string AssessmentData { get; set; }

        public string BaseUri { get; set; }
    }
}