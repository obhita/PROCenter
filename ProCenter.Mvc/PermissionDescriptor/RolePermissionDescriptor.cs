namespace ProCenter.Mvc.PermissionDescriptor
{
    #region

    using System.Web.Mvc;
    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    public class RolePermissionDescriptor : IInternalPermissionDescriptor
    {
        private readonly ResourceList _resourceList =
            new ResourceListBuilder().AddResource<RoleController>(RolePermission.RoleViewPermission,
                                                                  rlb =>
                                                                  rlb.AddResource("Edit", RolePermission.RoleEditPermission,
                                                                                  innerRlb =>
                                                                                  innerRlb.AddResource(HttpVerbs.Post.ToString().ToUpper(), RolePermission.RoleEditPermission))
                                                                     .AddResource("Create", RolePermission.RoleEditPermission)
                                                                     .AddResource("AddPermissions", RolePermission.RoleAddPermissionPermission)
                                                                     .AddResource("RemovePermissions", RolePermission.RoleRemovePermissionPermission));


        public ResourceList Resources
        {
            get { return _resourceList; }
        }

        public bool IsInternal { get { return false; } }
    }
}