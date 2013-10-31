namespace ProCenter.Service.Message.Metadata
{
    #region Using Statements

    using Pillar.Common.Metadata;

    #endregion

    /// <summary>
    ///     Metadata item for adding class to question
    /// </summary>
    public class ClassMetadataItem : IMetadataItem
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the class.
        /// </summary>
        /// <value>
        ///     The class.
        /// </value>
        public string Class { get; set; }

        #endregion
    }
}