namespace ProCenter.Mvc.Infrastructure.Permission
{
    using Pillar.Security.AccessControl;

    public class RolePermission
    {
        public static Permission RoleViewPermission = new Permission
            {
                Name = "securitynmodule/roleview",
            };

        public static Permission RoleEditPermission = new Permission
            {
                Name = "securitynmodule/roleedit",
            };

        public static Permission RoleAddPermissionPermission = new Permission
            {
                Name = "securitynmodule/roleaddpermission",
            };

        public static Permission RoleRemovePermissionPermission = new Permission
            {
                Name = "securitynmodule/roleremovepermission",
            };
    }
}