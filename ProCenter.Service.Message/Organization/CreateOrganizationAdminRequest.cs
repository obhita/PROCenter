namespace ProCenter.Service.Message.Organization
{
    using System;
    using Agatha.Common;

    public class CreateOrganizationAdminRequest : Request
    {
        public Guid OrganizationKey { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string BaseIdentityServerUri { get; set; }
        public string Token { get; set; }
    }
}
