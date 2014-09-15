namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;
    using System.ComponentModel.DataAnnotations;

    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>
    /// The PatientScoreRangeDto class.
    /// </summary>
    public class PatientScoreRangeDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        [Required]
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the assessment instance key.
        /// </summary>
        /// <value>
        /// The assessment instance key.
        /// </value>
        [Required]
        public Guid AssessmentInstanceKey { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        [Required]
        public Guid PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the assessment score.
        /// </summary>
        /// <value>
        /// The assessment score.
        /// </value>
        [Required]
        public string AssessmentScore { get; set; }

        /// <summary>
        /// Gets or sets the score date.
        /// </summary>
        /// <value>
        /// The score date.
        /// </value>
        [Required]
        public DateTime ScoreDate { get; set; }

        /// <summary>
        /// Gets or sets the patient birth date.
        /// </summary>
        /// <value>
        /// The patient birth date.
        /// </value>
        public DateTime PatientBirthDate { get; set; }

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
        /// Gets or sets the patient gender.
        /// </summary>
        /// <value>
        /// The patient gender.
        /// </value>
        public string PatientGender { get; set; }

        /// <summary>
        /// Gets or sets the score change.
        /// </summary>
        /// <value>
        /// The score change.
        /// </value>
        public string ScoreChange { get; set; }

        #endregion
    }
}