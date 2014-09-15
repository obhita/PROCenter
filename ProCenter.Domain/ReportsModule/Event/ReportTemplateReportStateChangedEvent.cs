namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template report state changed event class.</summary>
    public class ReportTemplateReportStateChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateReportStateChangedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="reportState">State of the report.</param>
        public ReportTemplateReportStateChangedEvent ( Guid key, int version, ReportState reportState )
            : base ( key, version )
        {
            ReportState = reportState;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the state of the report.
        /// </summary>
        /// <value>
        /// The state of the report.
        /// </value>
        public ReportState ReportState { get; private set; }

        #endregion
    }
}