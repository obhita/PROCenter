namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>The report parameters response class.</summary>
    public class ReportParametersResponse : Response
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; set; }

        #endregion
    }
}