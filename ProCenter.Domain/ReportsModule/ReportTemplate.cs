namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using Pillar.Common.Utility;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.ReportsModule.Event;

    #endregion

    /// <summary>The report template class.</summary>
    public class ReportTemplate : AggregateRootBase
    {
        private IReportTemplateRepository _reportTemplateRepository;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplate"/> class.
        /// </summary>
        public ReportTemplate ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplate"/> class.
        /// </summary>
        /// <param name="reportTemplateRepository">The report template repository.</param>
        public ReportTemplate(IReportTemplateRepository reportTemplateRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplate" /> class.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="name">The name.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="reportState">State of the report.</param>
        internal ReportTemplate ( Guid staffKey, string name, ReportType reportType, object parameters, ReportState reportState )
        {
            Check.IsNotNull ( name, () => Name );
            Check.IsNotNull ( reportType, () => ReportType );
            Check.IsNotNull ( parameters, () => Parameters );
            Check.IsNotNull ( reportState, () => ReportState );

            Key = CombGuid.NewCombGuid ();

            RaiseEvent ( new ReportTemplateCreatedEvent ( Key, Version, staffKey, name, reportType, parameters, reportState ) );
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid SystemAccountKey { get; protected set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; protected set; }

        /// <summary>
        /// Gets or sets the state of the report.
        /// </summary>
        /// <value>
        /// The state of the report.
        /// </value>
        public ReportState ReportState { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        public ReportType ReportType { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Changes the name.</summary>
        /// <param name="name">The name.</param>
        public void ChangeName ( string name )
        {
            RaiseEvent ( new ReportTemplateNameChangedEvent ( Key, Version, name ) );
        }

        /// <summary>Changes the parameters.</summary>
        /// <param name="parameters">The parameters.</param>
        public void ChangeParameters ( object parameters )
        {
            RaiseEvent ( new ReportTemplateParametersChangedEvent ( Key, Version, parameters ) );
        }

        /// <summary>Changes the state of the report.</summary>
        /// <param name="reportState">State of the report.</param>
        public void ChangeReportState ( ReportState reportState )
        {
            RaiseEvent ( new ReportTemplateReportStateChangedEvent ( Key, Version, reportState ) );
        }

        /// <summary>Changes the type of the report.</summary>
        /// <param name="reportType">Type of the report.</param>
        public void ChangeReportType ( ReportType reportType )
        {
            RaiseEvent ( new ReportTemplateReportTypeChangedEvent ( Key, Version, reportType ) );
        }

        #endregion

        #region Methods

        private void Apply ( ReportTemplateCreatedEvent reportTemplateCreatedEvent )
        {
            SystemAccountKey = reportTemplateCreatedEvent.SystemAccountKey;
            Name = reportTemplateCreatedEvent.Name;
            ReportType = reportTemplateCreatedEvent.ReportType;
            Parameters = reportTemplateCreatedEvent.Parameters;
            ReportState = reportTemplateCreatedEvent.ReportState;
        }

        private void Apply ( ReportTemplateNameChangedEvent reportTemplateChangeNameEvent )
        {
            Name = reportTemplateChangeNameEvent.Name;
        }

        private void Apply ( ReportTemplateReportTypeChangedEvent reportTemplateChangeReportTypeEvent )
        {
            ReportType = reportTemplateChangeReportTypeEvent.ReportType;
        }

        private void Apply ( ReportTemplateParametersChangedEvent reportTemplateChangeParametersEvent )
        {
            Parameters = reportTemplateChangeParametersEvent.Parameters;
        }

        private void Apply ( ReportTemplateReportStateChangedEvent reportTemplateChangeReportStateEvent )
        {
            ReportState = reportTemplateChangeReportStateEvent.ReportState;
        }

        #endregion
    }
}