namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The recent report created event class.</summary>
    public class RecentReportCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentReportCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="assessment">The assessment.</param>
        /// <param name="runDate">The run date.</param>
        /// <param name="parameters">The parameters.</param>
        public RecentReportCreatedEvent(Guid key, int version, string name, Guid systemAccountKey, string assessment, DateTime runDate, object parameters)
            : base ( key, version )
        {
            Name = name;
            SystemAccountKey = systemAccountKey;
            Assessment = assessment;
            RunDate = runDate;
            Parameters = parameters;
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
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; private set; }

        /// <summary>
        /// Gets the staff key.
        /// </summary>
        /// <value>
        /// The staff key.
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
    }
}