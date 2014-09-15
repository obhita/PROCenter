namespace ProCenter.Domain.ReportsModule.ChartAcrossAssessments
{
    using System;

    /// <summary>
    /// The PatientsWithSpecificResponseDataObject class.
    /// </summary>
    public class ChartAcrossAssessmentsDataObject
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
        public Guid? AssessmentInstanceKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the item definition code.
        /// </summary>
        /// <value>
        /// The item definition code.
        /// </value>
        public string ItemDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is code.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is code; otherwise, <c>false</c>.
        /// </value>
        public bool IsCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        /// <value>
        /// The type of the response.
        /// </value>
        public string ResponseType { get; set; }
    }
}
