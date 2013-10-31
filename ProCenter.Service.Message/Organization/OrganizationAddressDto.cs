namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using Common;
    using Common.Lookups;

    #endregion

    public class OrganizationAddressDto : KeyedDataTransferObject
    {
        #region Public Properties

        public AddressDto Address { get; set; }
        public bool IsPrimary { get; set; }
        public LookupDto OrganizationAddressType { get; set; }
        public int OriginalHash { get; set; }

        #endregion
    }
}