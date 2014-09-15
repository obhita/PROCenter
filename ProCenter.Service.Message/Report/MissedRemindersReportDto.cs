namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;

    using ProCenter.Service.Message.Common;

    #endregion

    /// <summary>
    /// The MissedRemindersReportDto Class.
    /// </summary>
    public class MissedRemindersReportDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment reminder key.
        /// </summary>
        /// <value>
        /// The assessment reminder key.
        /// </value>
        public Guid AssessmentReminderKey { get; set; }

        /// <summary>
        /// Gets or sets the recurrence key.
        /// </summary>
        /// <value>
        /// The recurrence key.
        /// </value>
        public Guid RecurrenceKey { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        /// The name of the patient.
        /// </value>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reminder start date.
        /// </summary>
        /// <value>
        /// The reminder start date.
        /// </value>
        public DateTime ReminderStartDate { get; set; }

        /// <summary>
        /// Gets or sets the assessment code.
        /// </summary>
        /// <value>
        /// The assessment code.
        /// </value>
        public string AssessmentCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets the reminder start date string.
        /// </summary>
        /// <value>
        /// The reminder start date string.
        /// </value>
        public string ReminderStartDateString
        {
            get { return ReminderStartDate.ToShortDateString(); }
        }

        #endregion
    }
}