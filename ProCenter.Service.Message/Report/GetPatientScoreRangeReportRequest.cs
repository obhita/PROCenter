namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>
    /// The GetNotCompletedAssessmentReportRequest class.
    /// </summary>
    public class GetNotCompletedAssessmentReportRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the not completed assessment parameters dto.
        /// </summary>
        /// <value>
        /// The not completed assessment parameters dto.
        /// </value>
        public NotCompletedAssessmentParametersDto NotCompletedAssessmentParametersDto { get; set; }

        #endregion
    }
}