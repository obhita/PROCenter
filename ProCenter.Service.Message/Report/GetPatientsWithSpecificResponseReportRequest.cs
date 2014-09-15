namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>
    /// The GetPatientsWithSpecificResponseReportRequest class.
    /// </summary>
    public class GetPatientsWithSpecificResponseReportRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the get patients with specific response report request dto.
        /// </summary>
        /// <value>
        /// The get patients with specific response report request dto.
        /// </value>
        public PatientsWithSpecificResponseParametersDto PatientsWithSpecificResponseParametersDto { get; set; }

        #endregion
    }
}