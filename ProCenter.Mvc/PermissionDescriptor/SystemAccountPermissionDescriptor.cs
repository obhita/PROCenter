namespace ProCenter.Mvc.PermissionDescriptor
{
    #region Using Statements

    using Common.Permission;
    using Controllers.Api;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    /// <summary>The system account permission descriptor class.</summary>
    public class SystemAccountPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder ()
            .AddResource<SystemAccountController> ( BasicAccessPermission.AccessUserInterfacePermission,
                rlb => rlb.AddResource ( "Lock", SystemAccountPermission.LockAccountPermission )
                    .AddResource ( "UnLock", SystemAccountPermission.LockAccountPermission )
                    .AddResource ( "ResetPassword", SystemAccountPermission.ResetPasswordPermission ) );

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether [is internal].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [is internal]; otherwise, <c>false</c>.
        /// </value>
        public bool IsInternal
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        public ResourceList Resources
        {
            get { return _resourceList; }
        }

        #endregion
    }
}