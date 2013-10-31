namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    public class OrganizaionPrimaryAddressChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        public OrganizaionPrimaryAddressChangedEvent ( Guid key, int version, int addressHash )
            : base ( key, version )
        {
            AddressHashCode = addressHash;
        }

        #endregion

        #region Public Properties

        public int AddressHashCode { get; private set; }

        #endregion
    }
}