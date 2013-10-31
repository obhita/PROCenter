namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using Common;
    using Pillar.Common.Bootstrapper;

    #endregion

    /// <summary>
    ///     Bootstrapper task for registering resource managers.
    /// </summary>
    public class RegisterResourcesBootstrapperTask : IOrderedBootstrapperTask
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterResourcesBootstrapperTask" /> class.
        /// </summary>
        /// <param name="resourcesManager">The resource manager provider.</param>
        public RegisterResourcesBootstrapperTask ( IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        public int Order { get; private set; }

        #region Public Methods and Operators

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute ()
        {
            _resourcesManager.Register<NidaSingleQuestionScreener> ( NidaSingleQuestionScreener.AssessmentCodedConcept.Code);
            _resourcesManager.Register<DrugAbuseScreeningTest> ( DrugAbuseScreeningTest.AssessmentCodedConcept.Code);
            _resourcesManager.Register<NidaAssessFurther>(NidaAssessFurther.AssessmentCodedConcept.Code);
            _resourcesManager.Register<NidaWorkflowPatientSummaryReport>();
        }

        #endregion
    }
}