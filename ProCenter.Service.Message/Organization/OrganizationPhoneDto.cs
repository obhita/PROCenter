namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System.ComponentModel.DataAnnotations;
    using Common;
    using Common.Lookups;
    using Domain.CommonModule;

    #endregion

    public class OrganizationPhoneDto : KeyedDataTransferObject
    {
        #region Public Properties

        public bool IsPrimary { get; set; }
        public LookupDto OrganizationPhoneType { get; set; }
        public PhoneDto Phone { get; set; }
        public int OriginalHash { get; set; }

        #endregion
    }
}