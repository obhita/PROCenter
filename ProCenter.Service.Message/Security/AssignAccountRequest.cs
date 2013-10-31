namespace ProCenter.Service.Message.Security
{
    using System;
    using Agatha.Common;

    public class AssignAccountRequest : Request
    {
        public Guid StaffKey { get; set; }
        public Guid PatientKey { get; set; }
        public Guid  OrganizationKey { get; set; }
        public SystemAccountDto SystemAccountDto { get; set; }
        public string BaseIdentityServerUri { get; set; }
        public string Token { get; set; }
    }
}