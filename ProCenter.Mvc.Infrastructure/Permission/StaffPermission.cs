namespace ProCenter.Mvc.Infrastructure.Permission
{
    #region

    using Pillar.Security.AccessControl;

    #endregion

    public class StaffPermission
    {
        public static Permission StaffViewPermission = new Permission
            {
                Name = "organizationmodule/staffview"
            };

        public static Permission StaffEditPermission = new Permission
            {
                Name = "organizationmodule/staffedit"
            };

        public static Permission StaffCreateAccountPermission = new Permission
            {
                Name = "organizationmodule/staffcreateaccount"
            };

        public static Permission StaffLinkAccountPermission = new Permission
            {
                Name = "organizationmodule/stafflinkaccount"
            };

        public static Permission StaffAddRolePermission = new Permission
            {
                Name = "organizationmodule/staffaddrole"
            };

        public static Permission StaffRemoveRolePermission = new Permission
            {
                Name = "organizationmodule/staffremoverole"
            };
    }
}