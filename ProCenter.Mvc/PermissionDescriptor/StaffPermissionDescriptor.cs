namespace ProCenter.Mvc.PermissionDescriptor
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    public class StaffPermissionDescriptor : IInternalPermissionDescriptor
    {
        private readonly ResourceList _resourceList =
            new ResourceListBuilder().AddResource<StaffController>(StaffPermission.StaffViewPermission,
                                                                   rlb =>
                                                                   rlb.AddResource("Edit", StaffPermission.StaffViewPermission,
                                                                                   innerRlb =>
                                                                                   innerRlb.AddResource(HttpVerbs.Post.ToString().ToUpper(), StaffPermission.StaffEditPermission))
                                                                      .AddResource("Create", StaffPermission.StaffEditPermission)
                                                                      .AddResource("CreateAccount", StaffPermission.StaffCreateAccountPermission)
                                                                      .AddResource("LinkAccount", StaffPermission.StaffLinkAccountPermission)
                                                                      .AddResource("AddRoles", StaffPermission.StaffAddRolePermission)
                                                                      .AddResource("RemoveRoles", StaffPermission.StaffRemoveRolePermission));


        public ResourceList Resources
        {
            get { return _resourceList; }
        }

        public bool IsInternal { get { return false; } }
    }
}