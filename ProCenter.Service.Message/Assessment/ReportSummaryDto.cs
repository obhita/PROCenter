namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Common;
    using Domain.AssessmentModule;

    #endregion

    /// <summary>
    ///     Data transfer object for report summary.
    /// </summary>
    public class ReportSummaryDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the report severity.
        /// </summary>
        /// <value>
        /// The report severity.
        /// </value>
        public ReportSeverity ReportSeverity { get; set; }

        /// <summary>
        /// Gets or sets the report status.
        /// </summary>
        /// <value>
        /// The report status.
        /// </value>
        public string ReportStatus { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can customize.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can customize; otherwise, <c>false</c>.
        /// </value>
        public bool CanCustomize { get; set; }

        /// <summary>
        ///     Gets or sets the created time.
        /// </summary>
        /// <value>
        ///     The created time.
        /// </value>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     Gets the created time string.
        /// </summary>
        /// <value>
        ///     The created time string.
        /// </value>
        public string CreatedTimeString
        {
            get { return CreatedTime.ToShortDateString (); }
        }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

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