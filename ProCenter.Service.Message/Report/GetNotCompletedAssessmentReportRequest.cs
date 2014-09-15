namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>
    /// The GetPatientScoreRangeReportRequest class.
    /// </summary>
    public class GetPatientScoreRangeReportRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the patient score range parameters dto.
        /// </summary>
        /// <value>
        /// The patient score range parameters dto.
        /// </value>
        public PatientScoreRangeParametersDto PatientScoreRangeParametersDto { get; set; }

        #endregion
    }
}