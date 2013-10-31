namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System.Collections.Generic;
    using Domain.AssessmentModule;

    #endregion

    /// <summary>
    ///     Data Transfer object for <see cref="ReportItem" />
    /// </summary>
    public class ReportItemDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the item metadata.
        /// </summary>
        /// <value>
        ///     The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the report items.
        /// </summary>
        /// <value>
        ///     The report items.
        /// </value>
        public IEnumerable<ReportItemDto> ReportItems { get; set; }

        /// <summary>
        ///     Gets the should show.
        /// </summary>
        /// <value>
        ///     The should show.
        /// </value>
        public bool? ShouldShow { get; set; }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text { get; set; }

        #endregion
    }
}