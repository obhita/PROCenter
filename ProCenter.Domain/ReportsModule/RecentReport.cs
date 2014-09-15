namespace ProCenter.Domain.ReportsModule
{
    using System;

    using Pillar.Common.Utility;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.ReportsModule.Event;

    /// <summary>
    /// The RecentReport class.
    /// </summary>
    public class RecentReport : AggregateRootBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentReport" /> class.
        /// </summary>
        public RecentReport ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentReport" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="assessment">The assessment.</param>
        /// <param name="runDate">The run date.</param>
        /// <param name="parameters">The parameters.</param>
        public RecentReport(string name, Guid systemAccountKey, string assessment, DateTime runDate, object parameters)
        {
            Check.IsNotNull ( name, () => Name );
            Check.IsNotNull ( parameters, () => Parameters );

            Key = CombGuid.NewCombGuid ();

            RaiseEvent(new RecentReportCreatedEvent(Key, Version, name, systemAccountKey, assessment, runDate, parameters));
        }

        #endregion

        #region Public Properties

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
        /// Gets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid SystemAccountKey { get; private set; }

        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <value>
        /// The assessment.
        /// </value>
        public string Assessment { get; private set; }

        /// <summary>
        /// Gets the run date.
        /// </summary>
        /// <value>
        /// The run date.
        /// </value>
        public DateTime RunDate { get; private set; }

        #endregion

        #region Methods

        private void Apply( RecentReportCreatedEvent recentReportCreated)
        {
            Name = recentReportCreated.Name;
            Parameters = recentReportCreated.Parameters;
            SystemAccountKey = recentReportCreated.SystemAccountKey;
            Assessment = recentReportCreated.Assessment;
            RunDate = recentReportCreated.RunDate;
        }

        #endregion
    }
}