namespace ProCenter.Domain.Nih
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    
    #endregion

    /// <summary>
    /// Class for NihHealthBehaviorsAssessment Profile Report.
    /// </summary>
    [ReportEngine ( typeof(NihHealthBehaviorsAssessment) )]
    public class NihProfileReport : IReportEngine
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IStaffRepository _staffRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NihProfileReport" /> class.
        /// </summary>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="staffRepository">The staff repository.</param>
        public NihProfileReport(
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
            var nihAssessment = new NihHealthBehaviorsAssessment(assessment);
            var reportDataCollection = new NihHealthBehaviorsAssessmentReportDataCollection
                                       {
                                            new NihHealthBehaviorsAssessmentReportData ( nihAssessment )
                                             {
                                                 SummaryReportInfo = new SummaryReportInfo
                                                                     {
                                                                         PatientId = patient.UniqueIdentifier,
                                                                         PatientName = patient.Name.FullName,
                                                                         StaffName = staff.Name.FullName,
                                                                         ScreeningDate = assessment.CreatedDate.ToShortDateString ()
                                                                     }
                                             }
                                       };
            var report = new NihHealthBehaviorsAssessmentPatientSummaryReport
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
    }
}