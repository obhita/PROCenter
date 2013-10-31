namespace ProCenter.Service.Message.Security
{
    using Agatha.Common;

    public class CreateRoleResponse : Response
    {
        public RoleDto Role { get; set; }
    }
}