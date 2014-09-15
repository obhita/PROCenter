using ProCenter.Primitive;

namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The assessment score over time parameters class.</summary>
    public class AssessmentScoreOverTimeParameters : BaseReportParameters, IReportModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        /// The assessment definition code.
        /// </value>
        public string AssessmentDefinitionCode { get; set; }

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