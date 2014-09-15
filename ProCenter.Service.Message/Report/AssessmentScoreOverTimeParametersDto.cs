using ProCenter.Primitive;

namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System.ComponentModel.DataAnnotations;

    #endregion

    /// <summary>The assessment score over time parameters dto class.</summary>
    public class AssessmentScoreOverTimeParametersDto : BaseParametersDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        /// The assessment definition code.
        /// </value>
        [Required]
        [Display ( Name = "Assessment" )]
        public string AssessmentDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        [Required]
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        /// The name of the patient.
        /// </value>
        public PersonName PatientName { get; set; }

        #endregion
    }
}