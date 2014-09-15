namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;

    using ProCenter.Service.Message.Common;

    #endregion

    /// <summary>
    /// The RecentReportsDto Class.
    /// </summary>
    public class ReportDefinitionDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the report definition key.
        /// </summary>
        /// <value>
        /// The report definition key.
        /// </value>
        public Guid ReportDefinitionKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is patient centric.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is patient centric; otherwise, <c>false</c>.
        /// </value>
        public bool IsPatientCentric { get; set; }

        #endregion
    }
}