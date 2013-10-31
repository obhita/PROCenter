namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using Common;

    #endregion

    /// <summary>
    ///     Organization Summary data transfer object.
    /// </summary>
    public class OrganizationSummaryDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the primary address.
        /// </summary>
        /// <value>
        ///     The primary address.
        /// </value>
        public AddressDto PrimaryAddress { get; set; }

        /// <summary>
        ///     Gets or sets the primary phone.
        /// </summary>
        /// <value>
        ///     The primary phone.
        /// </value>
        public string PrimaryPhone { get; set; }

        #endregion
    }
}