namespace ProCenter.Mvc.PermissionDescriptor
{
    #region Using Statements

    using System.Web.Mvc;
    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    public class PatientPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder ()
            .AddResource<PatientController> ( PatientPermission.PatientViewPermission,
                                              rlb => rlb.AddResource ( "Edit",
                                                                       PatientPermission.PatientViewPermission,
                                                                       innerrlb => innerrlb.AddResource ( HttpVerbs.Post.ToString ().ToUpper(), PatientPermission.PatientEditPermission ) )
                                                        .AddResource ( "Create", PatientPermission.PatientEditPermission ) );

        //todo: index action, create access

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