namespace ProCenter.Mvc.PermissionDescriptor
{
    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    public class OrganizationPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder ()
            .AddResource<OrganizationController> ( OrganizationPermission.OrganizationViewPermission,
                                                   rlb =>
                                                   rlb.AddResource ( "Edit",
                                                                     OrganizationPermission.OrganizationViewPermission,
                                                                     rlbinner => rlbinner.AddResource ( "POST", OrganizationPermission.OrganizationEditPermission ) ) );

        #endregion

        #region Public Properties

        public ResourceList Resources
        {
            get { return _resourceList; }
        }

        public bool IsInternal { get { return false; } }

        #endregion
    }
}