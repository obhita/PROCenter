namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;
    using Domain.AssessmentModule;

    #endregion

    /// <summary>
    ///     Data transfer object for <see cref="ReportModel" />
    /// </summary>
    public class ReportModelDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is customizable.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is customizable; otherwise, <c>false</c>.
        /// </value>
        public bool IsCustomizable { get; set; }

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
        ///     Gets or sets the item metadata.
        /// </summary>
        /// <value>
        ///     The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the report items.
        /// </summary>
        /// <value>
        ///     The report items.
        /// </value>
        public IEnumerable<ReportItemDto> ReportItems { get; set; }

        #endregion
    }
}