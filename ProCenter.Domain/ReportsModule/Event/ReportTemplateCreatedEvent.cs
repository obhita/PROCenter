namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template created event class.</summary>
    public class ReportTemplateCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="name">The name.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="reportState">State of the report.</param>
        public ReportTemplateCreatedEvent(Guid key, int version, Guid systemAccountKey, string name, ReportType reportType, object parameters, ReportState reportState)
            : base ( key, version )
        {
            SystemAccountKey = systemAccountKey;
            Name = name;
            ReportType = reportType;
            Parameters = parameters;
            ReportState = reportState;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid SystemAccountKey { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; private set; }

        /// <summary>
        /// Gets the state of the report.
        /// </summary>
        /// <value>
        /// The state of the report.
        /// </value>
        public ReportState ReportState { get; private set; }

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