namespace ProCenter.Infrastructure.Security
{
    using Pillar.Security.AccessControl;

    public class SystemAdministrationPermission
    {
        public static Permission SystemAdminPermission = new Permission { Name = "securitymodule/systemadmin" };
    }
}
