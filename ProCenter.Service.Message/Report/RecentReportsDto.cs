namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;

    using ProCenter.Service.Message.Common;

    #endregion

    /// <summary>
    /// The RecentReportsDto Class.
    /// </summary>
    public class RecentReportsDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the report key.
        /// </summary>
        /// <value>
        /// The report key.
        /// </value>
        public Guid ReportKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the report type.
        /// </summary>
        /// <value>
        /// The name of the report type.
        /// </value>
        public string ReportTypeName { get; set; }

        /// <summary>
        /// Gets or sets the assessment.
        /// </summary>
        /// <value>
        /// The assessment.
        /// </value>
        public string Assessment { get; set; }

        /// <summary>
        /// Gets or sets the run date.
        /// </summary>
        /// <value>
        /// The run date.
        /// </value>
        public DateTime RunDate { get; set; }

        /// <summary>
        /// Gets the run date string.
        /// </summary>
        /// <value>
        /// The run date string.
        /// </value>
        public string RunDateString
        {
            get { return RunDate.ToShortDateString(); }
        }

        #endregion
    }
}