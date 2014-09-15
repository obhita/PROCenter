namespace ProCenter.Domain.Psc
{
    #region Using Statements

    using System;
   
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    
    #endregion

    /// <summary>The Pediatric Sympton Checklist report engine class.</summary>
    [ReportEngine(typeof(PediatricSymptonChecklist))]
    public class PediatricSymptonChecklistSummaryReport : IReportEngine
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IStaffRepository _staffRepository;

        private PediatricSymptonChecklistReportData _pediatricSymptonChecklistReportData;

        private PediatricSymptonChecklist _pediatricSymptonChecklist;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PediatricSymptonChecklistSummaryReport" /> class.
        /// </summary>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="staffRepository">The staff repository.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        public PediatricSymptonChecklistSummaryReport(
            IPatientRepository patientRepository,
            IStaffRepository staffRepository,
            IAssessmentInstanceRepository assessmentInstanceRepository)
        {
            _patientRepository = patientRepository;
            _assessmentInstanceRepository = assessmentInstanceRepository;
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
            var patient = _patientRepository.GetByKey(assessment.PatientKey);
            var staff = _staffRepository.GetByKey(assessment.CreatedByStaffKey);
            _pediatricSymptonChecklist = new PediatricSymptonChecklist(assessment);

            _pediatricSymptonChecklistReportData = new PediatricSymptonChecklistReportData( _pediatricSymptonChecklist );
            PopulateReportData (patient, staff );
            var reportDataCollection = new PediatricSymptonChecklistReportDataCollection
                                       {
                                           _pediatricSymptonChecklistReportData
                                       };
            var report = new PediatricSymptonChecklistReport
                         {
                             DataSource = reportDataCollection,
                         };
            return report;
        }

        /// <summary>Gets the customization model.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>A <see cref="IReportModel" />.</returns>
        public IReportModel GetCustomizationModel ( Guid key, string reportName )
        {
            return null;
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

        /// <summary>
        /// Populates the report data.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="staff">The staff.</param>
        private void PopulateReportData (Patient patient, Staff staff)
        {
            _pediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal = GetAttentionProblemSubscaleTotal();
            _pediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal = GetAnxietyDepressionSubscaleTotal();
            _pediatricSymptonChecklistReportData.ConductProblemSubscaleTotal = GetConductProblemSubscaleTotal();
            _pediatricSymptonChecklistReportData.TotalScore = _pediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal +
                         _pediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal +
                         _pediatricSymptonChecklistReportData.ConductProblemSubscaleTotal +
                         GetOtherIssuesTotal();
            _pediatricSymptonChecklistReportData.Age = GetAge(patient);
            _pediatricSymptonChecklistReportData.StaffName = staff.Name.FullName;
            _pediatricSymptonChecklistReportData.ScreeningDate = _pediatricSymptonChecklist.AssessmentInstance.CreatedDate.ToLongDateString();
            if (string.IsNullOrWhiteSpace(_pediatricSymptonChecklist.AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription))
            {
                _pediatricSymptonChecklist.AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription = null;
            }
            _pediatricSymptonChecklistReportData.Answer4 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FidgetyUnableToSitStill);
            _pediatricSymptonChecklistReportData.Answer7 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ActsAsIfDrivenByAMotor);
            _pediatricSymptonChecklistReportData.Answer8 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DaydreamsTooMuch);
            _pediatricSymptonChecklistReportData.Answer9 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DistractedEasily);
            _pediatricSymptonChecklistReportData.Answer14 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleConcentrating);

            _pediatricSymptonChecklistReportData.Answer11 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsSadUnhappy);
            _pediatricSymptonChecklistReportData.Answer13 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsHopeless);
            _pediatricSymptonChecklistReportData.Answer19 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsDownOnHimOrHerself);
            _pediatricSymptonChecklistReportData.Answer22 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.WorriesAlot);
            _pediatricSymptonChecklistReportData.Answer27 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SeemsToBeHavingLessFun);

            _pediatricSymptonChecklistReportData.Answer16 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FightsWithOtherChildren);
            _pediatricSymptonChecklistReportData.Answer29 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNotListenToRules);
            _pediatricSymptonChecklistReportData.Answer31 = 
                GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNotUnderstandOtherPeoplesFeelings);
            _pediatricSymptonChecklistReportData.Answer32 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TeasesOthers);
            _pediatricSymptonChecklistReportData.Answer33 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.BlamesOthersForHisOrHerTroubles);
            _pediatricSymptonChecklistReportData.Answer34 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TakesThingsThatDoNotBelongToHimOrHer);
            _pediatricSymptonChecklistReportData.Answer35 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.RefusesToShare);

            _pediatricSymptonChecklistReportData.Answer1 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ComplainsOfAchesAndPains);
            _pediatricSymptonChecklistReportData.Answer2 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SpendsMoreTimeAlone);
            _pediatricSymptonChecklistReportData.Answer3 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TiresEasily);
            _pediatricSymptonChecklistReportData.Answer5 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleWithTeacher);
            _pediatricSymptonChecklistReportData.Answer6 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInSchool);
            _pediatricSymptonChecklistReportData.Answer10 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsAfraidOfNewSituations);
            _pediatricSymptonChecklistReportData.Answer12 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsIrritableAngry);
            _pediatricSymptonChecklistReportData.Answer15 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInFriends);
            _pediatricSymptonChecklistReportData.Answer17 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.AbsentFromSchool);
            _pediatricSymptonChecklistReportData.Answer18 = 
                GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SchoolGradesDropping);
            _pediatricSymptonChecklistReportData.Answer20 = 
                GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.VisitsTheDoctorWithDoctorFindingNothingWrong);
            _pediatricSymptonChecklistReportData.Answer21 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleSleeping);
            _pediatricSymptonChecklistReportData.Answer23 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.WantsToBeWithYouMoreThanBefore);
            _pediatricSymptonChecklistReportData.Answer24 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsHeOrSheIsBad);
            _pediatricSymptonChecklistReportData.Answer25 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TakesUnnecessaryRisks);
            _pediatricSymptonChecklistReportData.Answer26 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.GetsHurtFrequently);
            _pediatricSymptonChecklistReportData.Answer28 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ActsYoungerThanChildrenHisOrHerAge);
            _pediatricSymptonChecklistReportData.Answer30 = GetAnswerValue(_pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNoShowFeelings);
        }

        /// <summary>
        /// Gets the answer value.
        /// </summary>
        /// <param name="tf">The tf.</param>
        /// <returns>Returns the string with the Value and Name together.</returns>
        private string GetAnswerValue(Lookup tf)
        {
            return tf.Value + " - " + tf.CodedConcept.Name;
        }

        /// <summary>
        ///     Gets the age.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>The age of the patient baased on their birth date.</returns>
        private int GetAge(Patient patient)
        {
            if (patient.DateOfBirth == null || patient.DateOfBirth.Value == null)
            {
                return 0;
            }
            var now = DateTime.Today;
            var age = now.Year - patient.DateOfBirth.Value.Year;
            if (now < patient.DateOfBirth.Value.AddYears(age))
            {
                age--;
            }
            return age;
        }

        /// <summary>
        ///     Gets the attention problem subscale total.
        /// </summary>
        /// <returns>Returns an integer of the subtotal.</returns>
        private int GetAttentionProblemSubscaleTotal()
        {
            var total = _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FidgetyUnableToSitStill.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ActsAsIfDrivenByAMotor.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DaydreamsTooMuch.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DistractedEasily.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleConcentrating.Value;
            return int.Parse(total.ToString());
        }

        /// <summary>
        ///     Gets the anxiety depression subscale total.
        /// </summary>
        /// <returns>Returns an integer of the subtotal.</returns>
        private int GetAnxietyDepressionSubscaleTotal()
        {
            var total = _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsSadUnhappy.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsHopeless.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsDownOnHimOrHerself.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.WorriesAlot.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SeemsToBeHavingLessFun.Value;
            return int.Parse(total.ToString());
        }

        private int GetConductProblemSubscaleTotal()
        {
            var total = _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FightsWithOtherChildren.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNotListenToRules.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNotUnderstandOtherPeoplesFeelings.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TeasesOthers.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.BlamesOthersForHisOrHerTroubles.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TakesThingsThatDoNotBelongToHimOrHer.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.RefusesToShare.Value;
            return int.Parse(total.ToString());
        }

        /// <summary>
        /// Gets the other issues total.
        /// </summary>
        /// <returns>Returns a total for all other questions.</returns>
        private int GetOtherIssuesTotal()
        {
            var total = _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ComplainsOfAchesAndPains.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SpendsMoreTimeAlone.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TiresEasily.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleWithTeacher.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInSchool.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsAfraidOfNewSituations.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.IsIrritableAngry.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInFriends.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.AbsentFromSchool.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SchoolGradesDropping.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.VisitsTheDoctorWithDoctorFindingNothingWrong.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleSleeping.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.WantsToBeWithYouMoreThanBefore.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.FeelsHeOrSheIsBad.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.TakesUnnecessaryRisks.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.GetsHurtFrequently.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.ActsYoungerThanChildrenHisOrHerAge.Value +
                        _pediatricSymptonChecklistReportData.PediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.DoesNoShowFeelings.Value;
            return int.Parse(total.ToString());
        }
    }
}