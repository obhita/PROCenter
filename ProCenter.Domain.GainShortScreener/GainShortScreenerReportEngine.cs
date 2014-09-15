namespace ProCenter.Domain.GainShortScreener
{
    #region Using Statements

    using System;
    
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    
    #endregion

    /// <summary>The <see cref="GainShortScreener"/> profile report engine class.</summary>
    [ReportEngine ( typeof(GainShortScreener) )]
    public class GainShortScreenerProfileReport : IReportEngine
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IStaffRepository _staffRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreenerProfileReport" /> class.
        /// </summary>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="staffRepository">The staff repository.</param>
        public GainShortScreenerProfileReport(
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IPatientRepository patientRepository,
            IStaffRepository staffRepository)
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _patientRepository = patientRepository;
            _staffRepository = staffRepository;
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
            var assessment = _assessmentInstanceRepository.GetByKey ( key );
            var patient = _patientRepository.GetByKey ( assessment.PatientKey );
            var staff = _staffRepository.GetByKey ( assessment.CreatedByStaffKey.GetValueOrDefault() );
            var gainShortScreenerScore = assessment.Score.Value as GainShortScreenerScore;
            var reportDataCollection = new GainShortScreenerReportDataCollection
                                       {
                                           new GainShortScreenerReportData ( 
                                               gainShortScreenerScore, 
                                               new SummaryReportInfo
                                                                    {
                                                                        PatientId = patient.UniqueIdentifier,
                                                                        PatientName = patient.Name.FullName,
                                                                        StaffName =  staff.Name.FullName,
                                                                        ScreeningDate = assessment.CreatedDate.ToShortDateString (),
                                                                        Summary = GetSummary(gainShortScreenerScore, patient)
                                                                    })
                                       };
            var report = new GainShortScreenerSummaryReport
                         {
                             DataSource = reportDataCollection,
                         };
            return report;
        }

        /// <summary>
        /// Allways returns NULL as this report is not customizable.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        /// A NULL value.
        /// </returns>
        public IReportModel GetCustomizationModel(Guid key, string reportName, Guid? patientKey = null)
        {
            return null;
        }

        /// <summary>Does nothing as this report is not customizable.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateCustomizationModel ( Guid key, string reportName, string name, bool? shouldShow, string text )
        {
        }

        #endregion

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <param name="gainShortScreenerScore">The gain short screener score.</param>
        /// <param name="patient">The patient.</param>
        /// <returns>
        /// Returns an html string for the Summary text based on the severity of each sub screener.
        /// </returns>
        private string GetSummary(GainShortScreenerScore gainShortScreenerScore,
                                  Patient patient)
        {
            var recommend = string.Empty;
            var returnSummary = "During the past year " + patient.Name.FirstName + " was in the " +
                                GetScreenerSubScoreSeverity ( gainShortScreenerScore.InternalDisorder.PastYear ).CodedConcept.Name 
                                + " severity range on internalizing disorders; " +
                                "the " + GetScreenerSubScoreSeverity ( gainShortScreenerScore.ExternalDisorder.PastYear ).CodedConcept.Name
                                + " severity range on externalizing disorders; " +
                                "the " + GetScreenerSubScoreSeverity ( gainShortScreenerScore.SubstanceDisorder.PastYear ).CodedConcept.Name
                                + " severity range for substance use disorders; " +
                                "and the " + GetScreenerSubScoreSeverity ( gainShortScreenerScore.CriminalViolenceDisorder.PastYear ).CodedConcept.Name
                                + " severity range on the crime/violence sub-screeners. ";

            if ( GetScreenerSubScoreSeverity ( gainShortScreenerScore.InternalDisorder.PastYear ).CodedConcept.Name == DiagnosisLevel.High.CodedConcept.Name ||
                 GetScreenerSubScoreSeverity(gainShortScreenerScore.ExternalDisorder.PastYear).CodedConcept.Name == DiagnosisLevel.High.CodedConcept.Name)
            {
                recommend += "<li>referral for evaluation by a mental health service provider</li>";
            }
            if (GetScreenerSubScoreSeverity(gainShortScreenerScore.SubstanceDisorder.PastYear).CodedConcept.Name == DiagnosisLevel.High.CodedConcept.Name)
            {
                recommend += "<li>referral for evaluation by a substance abuse service provider</li>";
            }
            if (GetScreenerSubScoreSeverity(gainShortScreenerScore.CriminalViolenceDisorder.PastYear).CodedConcept.Name == DiagnosisLevel.High.CodedConcept.Name)
            {
                recommend += "<li>referral for anger management or legal services</li>";
            }

            if ( !string.IsNullOrWhiteSpace (recommend) )
            {
                recommend = "Given " + patient.Name.FirstName + "'s self-reported information the following is recommended: <br><br><br><ul>" + recommend + "</ul>";
            }

            return returnSummary + recommend;
        }

        /// <summary>
        /// Gets the screener sub score severity.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <returns>Returns the severity level as DiagnosisLevel object.</returns>
        private DiagnosisLevel GetScreenerSubScoreSeverity(int score)
        {
            if (score == 0)
            {
                return DiagnosisLevel.Low;
            }
            if (score <= 2)
            {
                return DiagnosisLevel.Medium;
            }
            return DiagnosisLevel.High;
        }
    }
}