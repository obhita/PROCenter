namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;

    using Agatha.Common;

    #endregion

    /// <summary>The get report parameters request class.</summary>
    public class GetReportParametersRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>
        /// The name of the report.
        /// </value>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the display name of the report.
        /// </summary>
        /// <value>
        /// The display name of the report.
        /// </value>
        public string ReportDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the template key.
        /// </summary>
        /// <value>
        /// The template key.
        /// </value>
        public Guid? TemplateKey { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid? PatientKey { get; set; }

        #endregion
    }
}