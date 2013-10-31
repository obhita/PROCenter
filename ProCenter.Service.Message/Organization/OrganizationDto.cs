namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;

    #endregion

    public class OrganizationDto : KeyedDataTransferObject
    {
        #region Public Properties

        public string Name { get; set; }
        public IEnumerable<OrganizationAddressDto> OrganizationAddresses { get; set; }
        public IEnumerable<OrganizationPhoneDto> OrganizationPhones { get; set; }

        #endregion
    }
}