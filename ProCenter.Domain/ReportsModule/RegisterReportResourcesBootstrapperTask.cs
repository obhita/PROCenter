namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using ProCenter.Common;
    using ProCenter.Domain.ReportsModule.ChartAcrossAssessments;
    using ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;
    using ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport;

    #endregion

    /// <summary>The register report resources bootstrapper task class.</summary>
    public class RegisterReportResourcesBootstrapperTask : IOrderedBootstrapperTask
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterReportResourcesBootstrapperTask" /> class.
        /// </summary>
        /// <param name="resourcesManager">The resource manager provider.</param>
        public RegisterReportResourcesBootstrapperTask ( IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute ()
        {
            _resourcesManager.Register<AssessmentScoreOverTime> ();
            _resourcesManager.Register<PatientScoreRange>();
            _resourcesManager.Register<NotCompletedAssessment>();
            _resourcesManager.Register<PatientsWithSpecificResponse>();
            _resourcesManager.Register<PatientsWithSpecificResponseAcrossAssessments>();
        }

        #endregion
    }
}