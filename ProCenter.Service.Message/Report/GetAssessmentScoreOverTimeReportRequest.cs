namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>The get assessment score over time report request class.</summary>
    public class GetAssessmentScoreOverTimeReportRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment score over time parameters dto.
        /// </summary>
        /// <value>
        /// The assessment score over time parameters dto.
        /// </value>
        public AssessmentScoreOverTimeParametersDto AssessmentScoreOverTimeParametersDto { get; set; }

        #endregion
    }
}