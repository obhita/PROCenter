namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using AssessmentModule;

    #endregion

    /// <summary>
    /// Event for when workflow is completed.
    /// </summary>
    public class WorkflowMessageReportReadyEvent : MessageEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowMessageReportReadyEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="workflowReport">The workflow report.</param>
        /// <param name="patientKey">The patient key.</param>
        public WorkflowMessageReportReadyEvent ( Guid key, MessageType messageType, ReportModel workflowReport, Guid patientKey )
            : base ( key, messageType )
        {
            WorkflowReport = workflowReport;
            PatientKey = patientKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the workflow report.
        /// </summary>
        /// <value>
        /// The workflow report.
        /// </value>
        public ReportModel WorkflowReport { get; private set; }

        /// <summary>
        /// Gets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        #endregion
    }
}