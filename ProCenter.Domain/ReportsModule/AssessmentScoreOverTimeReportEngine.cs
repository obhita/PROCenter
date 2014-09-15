namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;
    using System.Linq;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.PatientModule;

    #endregion

    /// <summary>The assessment score over time report engine class.</summary>
    [ReportEngine ( ReportNames.AssessmentScoreOverTime )]
    public class AssessmentScoreOverTimeReportEngine : IReportEngine
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IReportTemplateRepository _reportTemplateRepository;

        private readonly IRecentReportRepository _recentReportRepository;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentScoreOverTimeReportEngine" /> class.
        /// </summary>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="reportTemplateRepository">The report template repository.</param>
        /// <param name="reportHistoryRepository">The report history repository.</param>
        public AssessmentScoreOverTimeReportEngine (
            IPatientRepository patientRepository,
            IResourcesManager resourcesManager,
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IReportTemplateRepository reportTemplateRepository,
            IRecentReportRepository reportHistoryRepository)
        {
            _patientRepository = patientRepository;
            _resourcesManager = resourcesManager;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _reportTemplateRepository = reportTemplateRepository;
            _recentReportRepository = reportHistoryRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Generates the specified key.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="IReport" />.</returns>
        /// <exception cref="System.ArgumentException">Invalid parameters.</exception>
        public IReport Generate ( Guid key, string reportName, object parameters = null )
        {
            AssessmentScoreOverTimeParameters assessmentScoreOverTimeParameters;
            if ( key == Guid.Empty )
            {
                assessmentScoreOverTimeParameters = parameters as AssessmentScoreOverTimeParameters;
            }
            else
            {
                assessmentScoreOverTimeParameters = GetCustomizationModel ( key, reportName ) as AssessmentScoreOverTimeParameters;
            }
            if ( assessmentScoreOverTimeParameters == null )
            {
                throw new ArgumentException ( string.Format ( "Invalid parameters: {0}", parameters ), "parameters" );
            }

            var patient = _patientRepository.GetByKey ( assessmentScoreOverTimeParameters.PatientKey );
            var start = assessmentScoreOverTimeParameters.StartDate;
            var end = assessmentScoreOverTimeParameters.EndDate;
            if ( assessmentScoreOverTimeParameters.TimePeriod != null )
            {
                assessmentScoreOverTimeParameters.TimePeriod.GetRange ( out start, out end );
            }
            var scores = _assessmentInstanceRepository.GetAssessmentScores (
                                                                            assessmentScoreOverTimeParameters.PatientKey,
                assessmentScoreOverTimeParameters.AssessmentDefinitionCode,
                start.Value,
                end.Value );
            var reportModel = new AssessmentScoreOverTimeData ()
                              {
                                  AssessmentName = assessmentScoreOverTimeParameters.AssessmentName,
                                  PatientName = patient.Name.FullName,
                                  Scores = scores.ToList ()
                              };

            var reportDataCollection = new AssessmentScoreOverTimeDataCollection ()
                                       {
                                           reportModel
                                       };
            var report = new AssessmentScoreOverTimeReport
                         {
                             DataSource = reportDataCollection,
                         };
            return report;
        }
        
        /// <summary>
        /// Gets the customization model.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        /// A <see cref="IReportModel"/>.
        /// </returns>
        public IReportModel GetCustomizationModel(Guid key, string reportName, Guid? patientKey = null)
        {
            AssessmentScoreOverTimeParameters parameters = null;
            if (key != Guid.Empty)
            {
                var reportTemplate = _reportTemplateRepository.GetByKey(key);
                if (reportTemplate != null)
                {
                    parameters = reportTemplate.Parameters as AssessmentScoreOverTimeParameters;
                }
                if ( parameters == null )
                {
                    var reporthistory = _recentReportRepository.GetByKey(key);
                    if (reporthistory != null)
                    {
                        parameters = reporthistory.Parameters as AssessmentScoreOverTimeParameters;
                    }
                }
            }
            else
            {
                parameters = new AssessmentScoreOverTimeParameters {ReportName = reportName};
            }

            if (parameters != null && patientKey.HasValue)
            {
                parameters.PatientName = _patientRepository.GetByKey(patientKey.Value).Name;
            }

            return parameters;
        }

        /// <summary>Updates the customization model.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateCustomizationModel ( Guid key, string reportName, string name, bool? shouldShow, string text )
        {
        }

        #endregion
    }
}