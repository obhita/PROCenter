namespace ProCenter.Domain.OrganizationModule
{
    using System;
    using Primitive;

    public class StaffFactory : IStaffFactory
    {
        public Staff Create(Guid organizationKey, PersonName name)
        {
            return new Staff(organizationKey, name);
        }
    }
}