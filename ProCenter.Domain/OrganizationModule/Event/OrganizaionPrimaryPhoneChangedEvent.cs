namespace ProCenter.Domain.OrganizationModule.Event
{
    using System;
    using CommonModule;

    public class OrganizaionPrimaryPhoneChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        public OrganizaionPrimaryPhoneChangedEvent(Guid key, int version, int phoneHashCode)
            : base(key, version)
        {
            PhoneHashCode = phoneHashCode;
        }

        #endregion

        #region Public Properties

        public int PhoneHashCode { get; private set; }

        #endregion
    }
}