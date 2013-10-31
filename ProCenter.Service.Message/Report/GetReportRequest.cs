namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Request for report
    /// </summary>
    public class GetReportRequest : Request
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the report.
        /// </summary>
        /// <value>
        ///     The name of the report.
        /// </value>
        public string ReportName { get; set; }

        /// <summary>
        ///     Gets or sets the source key.
        /// </summary>
        /// <value>
        ///     The source key.
        /// </value>
        public Guid SourceKey { get; set; }

        #endregion
    }
}