namespace ProCenter.Service.Message.Metadata
{
    #region Using Statements

    using Pillar.Common.Metadata;

    #endregion

    /// <summary>
    ///     Meta data item for dispay order.
    /// </summary>
    public class DisplayOrderMetadataItem : IMetadataItem
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>
        ///     The order.
        /// </value>
        public int Order { get; set; }

        #endregion
    }
}