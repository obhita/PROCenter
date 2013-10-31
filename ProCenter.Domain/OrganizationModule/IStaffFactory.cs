namespace ProCenter.Domain.OrganizationModule
{
    using System;
    using Primitive;

    public interface IStaffFactory
    {
        Staff Create(Guid organizationKey, PersonName name);
    }
}