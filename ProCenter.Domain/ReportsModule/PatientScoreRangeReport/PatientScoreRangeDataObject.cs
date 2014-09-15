namespace ProCenter.Domain.ReportsModule.PatientScoreRangeReport
{
    using System;

    /// <summary>
    /// The PatientScoreRangeDataObject class.
    /// </summary>
    public class PatientScoreRangeDataObject
    {
        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        /// The name of the patient.
        /// </value>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        /// The first name of the patient.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        /// The last name of the patient.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the patient birth date.
        /// </summary>
        /// <value>
        /// The patient birth date.
        /// </value>
        public DateTime PatientBirthDate { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public string Score { get; set; }

        /// <summary>
        /// Gets or sets the change.
        /// </summary>
        /// <value>
        /// The change.
        /// </value>
        public string Change { get; set; }

        /// <summary>
        /// Gets or sets the assessment date.
        /// </summary>
        /// <value>
        /// The assessment date.
        /// </value>
        public string AssessmentDate { get; set; }

        /// <summary>
        /// Gets or sets the assessment instance key.
        /// </summary>
        /// <value>
        /// The assessment instance key.
        /// </value>
        public Guid AssessmentInstanceKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; set; }
    }
}
