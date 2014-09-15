namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template report type changed event class.</summary>
    public class ReportTemplateReportTypeChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateReportTypeChangedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="reportType">Type of the report.</param>
        public ReportTemplateReportTypeChangedEvent ( Guid key, int version, ReportType reportType )
            : base ( key, version )
        {
            ReportType = reportType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        public ReportType ReportType { get; private set; }

        #endregion
    }
}