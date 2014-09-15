namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System.Collections.Generic;

    using ProCenter.Common.Report;

    #endregion

    /// <summary>The assessment score over time data class.</summary>
    public class AssessmentScoreOverTimeData
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public ReportString AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        /// The name of the patient.
        /// </value>
        public ReportString PatientName { get; set; }

        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public IList<ScoreData> Scores { get; set; }

        #endregion
    }
}