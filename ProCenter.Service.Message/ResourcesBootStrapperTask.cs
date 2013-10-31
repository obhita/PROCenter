namespace ProCenter.Service.Message
{
    #region

    using Assessment;
    using Common;
    using Organization;
    using Pillar.Common.Bootstrapper;
    using ProCenter.Common;

    #endregion

    public class ResourcesBootStrapperTask : IOrderedBootstrapperTask
    {
        private readonly IResourcesManager _resourcesManager;

        public int Order { get; private set; }

        public ResourcesBootStrapperTask(IResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        public void Execute()
        {
            _resourcesManager.Register<CommonResources>();
            _resourcesManager.Register<OrganizationResources>();
            _resourcesManager.Register<AssessmentResources>();
        }
    }
}