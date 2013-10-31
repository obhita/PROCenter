namespace ProCenter.Service.Message.Metadata
{
    #region Using Statements

    using Pillar.Common.Metadata;

    #endregion

    /// <summary>
    ///     Meta data for Item Template
    /// </summary>
    public class ItemTemplateMetadataItem : UiMetadataItem
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the template.
        /// </summary>
        /// <value>
        ///     The name of the template.
        /// </value>
        public string TemplateName { get; set; }

        #endregion
    }
}