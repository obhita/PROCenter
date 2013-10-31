namespace ProCenter.Service.Message.Organization
{
    using Agatha.Common;
    using Security;

    public class CreateOrganizationAdminResponse : Response
    {
        public SystemAccountDto SystemAccountDto { get; set; }
    }
}
