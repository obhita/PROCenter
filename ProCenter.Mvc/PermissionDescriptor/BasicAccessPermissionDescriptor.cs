namespace ProCenter.Mvc.PermissionDescriptor
{
    #region Using Statements

    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    public class BasicAccessPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder ()
            .AddResource<HomeController> ( BasicAccessPermission.AccessUserInterfacePermission )
            .AddResource<AccountController>(BasicAccessPermission.AccessUserInterfacePermission)
            .AddResource<ErrorController>(BasicAccessPermission.AccessUserInterfacePermission);

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