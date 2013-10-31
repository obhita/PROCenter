namespace ProCenter.Service.Message.Organization
{
    using System;
    using Agatha.Common;
    using Primitive;

    public class CreateStaffRequest: Request
    {
        public Guid OrganizationKey { get; set; }
        public PersonName Name { get; set; }
    }
}