namespace ProCenter.Mvc.PermissionDescriptor
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure.Permission;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    public class AssessmentPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder()
            .AddResource<AssessmentController>(AssessmentPermission.AssessmentViewPermission,
                                               rlb => rlb.AddResource("Edit",
                                                                      AssessmentPermission.AssessmentViewPermission,
                                                                      innerrlb =>
                                                                      innerrlb.AddResource(
                                                                          HttpVerbs.Post.ToString().ToUpper(),
                                                                          AssessmentPermission.AssessmentEditPermission))
                                                         .AddResource("Create",
                                                                      AssessmentPermission.AssessmentEditPermission))
            .AddResource<WorkflowMessageController>(AssessmentPermission.AssessmentEditPermission)
            .AddResource<AssessmentReminderController>(AssessmentPermission.AssessmentReminderViewPermission,
                                               rlb => rlb.AddResource("Edit",
                                                                      AssessmentPermission.AssessmentReminderViewPermission,
                                                                      innerrlb =>
                                                                      innerrlb.AddResource(
                                                                          HttpVerbs.Post.ToString().ToUpper(),
                                                                          AssessmentPermission.AssessmentReminderEditPermission))
                                                         .AddResource("Create",
                                                                      AssessmentPermission.AssessmentReminderEditPermission)
                                                         .AddResource("Get",
                                                                      AssessmentPermission.AssessmentReminderViewPermission))
            .AddResource<ReportController>(AssessmentPermission.ReportViewPermission,
                                               rlb => rlb.AddResource("Customize",
                                                                      AssessmentPermission.ReportEditPermission));

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