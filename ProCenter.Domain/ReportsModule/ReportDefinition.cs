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
    public class ReportDefinition : AggregateRootBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDefinition"/> class.
        /// </summary>
        public ReportDefinition ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDefinition" /> class.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isPatientCentric">If set to <c>true</c> [is patient centric].</param>
        internal ReportDefinition(Guid staffKey, string reportName, string displayName, bool isPatientCentric)
        {
            Check.IsNotNull(reportName, () => ReportName);
            Check.IsNotNull(displayName, () => DisplayName);
            Check.IsNotNull(isPatientCentric, () => IsPatientCentric);
            Key = CombGuid.NewCombGuid ();

            RaiseEvent ( new ReportDefinitionCreatedEvent ( Key, Version, staffKey, reportName, displayName, isPatientCentric ) );
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
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>
        /// The name of the report.
        /// </value>
        public string ReportName { get; protected set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is patient centric.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is patient centric; otherwise, <c>false</c>.
        /// </value>
        public bool IsPatientCentric { get; protected set; }

        #endregion

        #region Public Methods and Operators

        #endregion

        #region Methods

        private void Apply ( ReportDefinitionCreatedEvent reportDefinitionCreatedEvent )
        {
            SystemAccountKey = reportDefinitionCreatedEvent.SystemAccountKey;
            ReportName = reportDefinitionCreatedEvent.ReportName;
            DisplayName = reportDefinitionCreatedEvent.DisplayName;
            IsPatientCentric = reportDefinitionCreatedEvent.IsPatientCentric;
        }

        #endregion
    }
}