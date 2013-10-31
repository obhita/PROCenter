namespace ProCenter.Service.Message.Metadata
{
    #region

    using Pillar.Common.Metadata;

    #endregion

    /// <summary>
    ///     Metadata Item dto for completeness
    /// </summary>
    public class RequiredForCompletenessMetadataItem : IMetadataItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequiredForCompletenessMetadataItem" /> class.
        /// </summary>
        /// <param name="completenessCategory">The completeness category.</param>
        public RequiredForCompletenessMetadataItem(string completenessCategory)
        {
            CompletenessCategory = completenessCategory;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the completeness category.
        /// </summary>
        /// <value>
        ///     The completeness category.
        /// </value>
        public string CompletenessCategory { get; private set; }

        #endregion
    }
}