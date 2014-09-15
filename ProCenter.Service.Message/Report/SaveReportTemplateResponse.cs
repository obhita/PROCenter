namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>The save report template response class.</summary>
    public class SaveReportTemplateResponse : Response
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the report template.
        /// </summary>
        /// <value>
        /// The report template.
        /// </value>
        public ReportTemplateDto ReportTemplate { get; set; }

        #endregion
    }
}