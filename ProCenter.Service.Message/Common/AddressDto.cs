namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System.ComponentModel.DataAnnotations;
    using Lookups;

    #endregion

    /// <summary>
    ///     Address data transfer object.
    /// </summary>
    public class AddressDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>
        ///     The name of the city.
        /// </value>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets the first street address.
        /// </summary>
        /// <value>
        ///     The first street address.
        /// </value>
        public string FirstStreetAddress { get; set; }

        /// <summary>
        ///     Gets or sets the postal code.
        /// </summary>
        /// <value>
        ///     The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        ///     Gets or sets the second street address.
        /// </summary>
        /// <value>
        ///     The second street address.
        /// </value>
        [Display(Name = "Street address line 2")]
        public string SecondStreetAddress { get; set; }

        /// <summary>
        ///     Gets or sets the state province.
        /// </summary>
        /// <value>
        ///     The state province.
        /// </value>
        public LookupDto StateProvince { get; set; }

        #endregion
    }
}