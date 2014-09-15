namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>The report template dto class.</summary>
    public class ReportTemplateDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the display name of the report type.
        /// </summary>
        /// <value>
        /// The display name of the report type.
        /// </value>
        public string ReportDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the report type.
        /// </summary>
        /// <value>
        /// The name of the report type.
        /// </value>
        public string ReportTypeName { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name parameters.
        /// </summary>
        /// <value>
        /// The name parameters.
        /// </value>
        public string NameParameters { get; set; }

        /// <summary>
        /// Gets or sets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid SystemAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; set; }

        /// <summary>
        /// Gets or sets the state of the report.
        /// </summary>
        /// <value>
        /// The state of the report.
        /// </value>
        public LookupDto ReportState { get; set; }

        /// <summary>
        /// Gets or sets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        public ReportType ReportType { get; set; }

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