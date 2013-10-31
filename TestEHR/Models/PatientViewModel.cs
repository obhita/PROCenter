namespace TestEHR.Models
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    #endregion

    public class PatientViewModel
    {
        public PatientViewModel()
        {
            AssessmentKeys = new List<Guid>();
        }
        public string Gender { get; set; }
        public Guid PatientKey { get; set; }
        public string Name { get; set; }
        public List<Guid> AssessmentKeys { get; set; }
    }

    public class PatientList
    {
        public PatientList()
        {
            Patients = new List<PatientViewModel>();
        }

        public List<PatientViewModel> Patients { get; set; }
    }
}