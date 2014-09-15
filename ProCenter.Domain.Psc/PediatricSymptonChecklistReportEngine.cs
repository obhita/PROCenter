namespace ProCenter.Domain.Psc
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
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
            var staff = _staffRepository.GetByKey(assessment.CreatedByStaffKey.GetValueOrDefault());
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

        /// <summary>
        /// Gets the customization model.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        /// A <see cref="IReportModel" />.
        /// </returns>
        public IReportModel GetCustomizationModel(Guid key, string reportName, Guid? patientKey = null)
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
            var group = _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup;
            _pediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal = GetAttentionProblemSubscaleTotal();
            _pediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal = GetAnxietyDepressionSubscaleTotal();
            _pediatricSymptonChecklistReportData.ConductProblemSubscaleTotal = GetConductProblemSubscaleTotal();
            _pediatricSymptonChecklistReportData.TotalScore = _pediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal +
                         _pediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal +
                         _pediatricSymptonChecklistReportData.ConductProblemSubscaleTotal +
                         GetOtherIssuesTotal();

            var values = new List<TimeFrequency>
                             {
                                 _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.HasTroubleWithTeacher,
                                 _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInSchool,
                                 _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.AbsentFromSchool,
                                 _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup.SchoolGradesDropping
                             };
            _pediatricSymptonChecklistReportData.TotalScore -= Common.AdjustScoreBasedOnAge(patient, values);

            _pediatricSymptonChecklistReportData.Age = Common.GetAge(patient);
            _pediatricSymptonChecklistReportData.StaffName = staff.Name.FullName;
            _pediatricSymptonChecklistReportData.ScreeningDate = _pediatricSymptonChecklist.AssessmentInstance.CreatedDate.ToLongDateString();
            if (string.IsNullOrWhiteSpace(_pediatricSymptonChecklist.AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription))
            {
                _pediatricSymptonChecklist.AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription = null;
            }
            _pediatricSymptonChecklistReportData.Answer4 = Common.GetAnswerValue(group.FidgetyUnableToSitStill);
            _pediatricSymptonChecklistReportData.Answer7 = Common.GetAnswerValue(group.ActsAsIfDrivenByAMotor);
            _pediatricSymptonChecklistReportData.Answer8 = Common.GetAnswerValue(group.DaydreamsTooMuch);
            _pediatricSymptonChecklistReportData.Answer9 = Common.GetAnswerValue(group.DistractedEasily);
            _pediatricSymptonChecklistReportData.Answer14 = Common.GetAnswerValue(group.HasTroubleConcentrating);

            _pediatricSymptonChecklistReportData.Answer11 = Common.GetAnswerValue(group.FeelsSadUnhappy);
            _pediatricSymptonChecklistReportData.Answer13 = Common.GetAnswerValue(group.FeelsHopeless);
            _pediatricSymptonChecklistReportData.Answer19 = Common.GetAnswerValue(group.IsDownOnHimOrHerself);
            _pediatricSymptonChecklistReportData.Answer22 = Common.GetAnswerValue(group.WorriesAlot);
            _pediatricSymptonChecklistReportData.Answer27 = Common.GetAnswerValue(group.SeemsToBeHavingLessFun);

            _pediatricSymptonChecklistReportData.Answer16 = Common.GetAnswerValue(group.FightsWithOtherChildren);
            _pediatricSymptonChecklistReportData.Answer29 = Common.GetAnswerValue(group.DoesNotListenToRules);
            _pediatricSymptonChecklistReportData.Answer31 = Common.GetAnswerValue(group.DoesNotUnderstandOtherPeoplesFeelings);
            _pediatricSymptonChecklistReportData.Answer32 = Common.GetAnswerValue(group.TeasesOthers);
            _pediatricSymptonChecklistReportData.Answer33 = Common.GetAnswerValue(group.BlamesOthersForHisOrHerTroubles);
            _pediatricSymptonChecklistReportData.Answer34 = Common.GetAnswerValue(group.TakesThingsThatDoNotBelongToHimOrHer);
            _pediatricSymptonChecklistReportData.Answer35 = Common.GetAnswerValue(group.RefusesToShare);

            // if 4-5 years old then this should say N/A
            if (_pediatricSymptonChecklistReportData.Age >= 4 && _pediatricSymptonChecklistReportData.Age <= 5)
            {
                _pediatricSymptonChecklistReportData.Answer5 = "0 - N/A";
                _pediatricSymptonChecklistReportData.Answer6 = "0 - N/A";
                _pediatricSymptonChecklistReportData.Answer17 = "0 - N/A";
                _pediatricSymptonChecklistReportData.Answer18 = "0 - N/A";
            }
            else
            {
                _pediatricSymptonChecklistReportData.Answer5 = Common.GetAnswerValue(group.HasTroubleWithTeacher);
                _pediatricSymptonChecklistReportData.Answer6 = Common.GetAnswerValue(group.LessInterestedInSchool);
                _pediatricSymptonChecklistReportData.Answer17 = Common.GetAnswerValue(group.AbsentFromSchool);
                _pediatricSymptonChecklistReportData.Answer18 = Common.GetAnswerValue(group.SchoolGradesDropping);
            }

            _pediatricSymptonChecklistReportData.Answer1 = Common.GetAnswerValue(group.ComplainsOfAchesAndPains);
            _pediatricSymptonChecklistReportData.Answer2 = Common.GetAnswerValue(group.SpendsMoreTimeAlone);
            _pediatricSymptonChecklistReportData.Answer3 = Common.GetAnswerValue(group.TiresEasily);
            _pediatricSymptonChecklistReportData.Answer10 = Common.GetAnswerValue(group.IsAfraidOfNewSituations);
            _pediatricSymptonChecklistReportData.Answer12 = Common.GetAnswerValue(group.IsIrritableAngry);
            _pediatricSymptonChecklistReportData.Answer15 = Common.GetAnswerValue(group.LessInterestedInFriends);
            _pediatricSymptonChecklistReportData.Answer20 = Common.GetAnswerValue(group.VisitsTheDoctorWithDoctorFindingNothingWrong);
            _pediatricSymptonChecklistReportData.Answer21 = Common.GetAnswerValue(group.HasTroubleSleeping);
            _pediatricSymptonChecklistReportData.Answer23 = Common.GetAnswerValue(group.WantsToBeWithYouMoreThanBefore);
            _pediatricSymptonChecklistReportData.Answer24 = Common.GetAnswerValue(group.FeelsHeOrSheIsBad);
            _pediatricSymptonChecklistReportData.Answer25 = Common.GetAnswerValue(group.TakesUnnecessaryRisks);
            _pediatricSymptonChecklistReportData.Answer26 = Common.GetAnswerValue(group.GetsHurtFrequently);
            _pediatricSymptonChecklistReportData.Answer28 = Common.GetAnswerValue(group.ActsYoungerThanChildrenHisOrHerAge);
            _pediatricSymptonChecklistReportData.Answer30 = Common.GetAnswerValue(group.DoesNoShowFeelings);
        }

        /// <summary>
        ///     Gets the attention problem subscale total.
        /// </summary>
        /// <returns>Returns an integer of the subtotal.</returns>
        private int GetAttentionProblemSubscaleTotal()
        {
            var group = _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup;

            var total = group.FidgetyUnableToSitStill.Value +
                        group.ActsAsIfDrivenByAMotor.Value +
                        group.DaydreamsTooMuch.Value +
                        group.DistractedEasily.Value +
                        group.HasTroubleConcentrating.Value;
            return int.Parse(total.ToString());
        }

        /// <summary>
        ///     Gets the anxiety depression subscale total.
        /// </summary>
        /// <returns>Returns an integer of the subtotal.</returns>
        private int GetAnxietyDepressionSubscaleTotal()
        {
            var group = _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup;

            var total = group.FeelsSadUnhappy.Value +
                        group.FeelsHopeless.Value +
                        group.IsDownOnHimOrHerself.Value +
                        group.WorriesAlot.Value +
                        group.SeemsToBeHavingLessFun.Value;
            return int.Parse(total.ToString());
        }

        private int GetConductProblemSubscaleTotal()
        {
            var group = _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup;

            var total = group.FightsWithOtherChildren.Value +
                        group.DoesNotListenToRules.Value +
                        group.DoesNotUnderstandOtherPeoplesFeelings.Value +
                        group.TeasesOthers.Value +
                        group.BlamesOthersForHisOrHerTroubles.Value +
                        group.TakesThingsThatDoNotBelongToHimOrHer.Value +
                        group.RefusesToShare.Value;
            return int.Parse(total.ToString());
        }

        /// <summary>
        /// Gets the other issues total.
        /// </summary>
        /// <returns>Returns a total for all other questions.</returns>
        private int GetOtherIssuesTotal()
        {
            var group = _pediatricSymptonChecklist.ChildsEmotionalAndBehaviorProblemsGroup;

            var total = group.ComplainsOfAchesAndPains.Value +
                        group.SpendsMoreTimeAlone.Value +
                        group.TiresEasily.Value +
                        group.HasTroubleWithTeacher.Value +
                        group.LessInterestedInSchool.Value +
                        group.IsAfraidOfNewSituations.Value +
                        group.IsIrritableAngry.Value +
                        group.LessInterestedInFriends.Value +
                        group.AbsentFromSchool.Value +
                        group.SchoolGradesDropping.Value +
                        group.VisitsTheDoctorWithDoctorFindingNothingWrong.Value +
                        group.HasTroubleSleeping.Value +
                        group.WantsToBeWithYouMoreThanBefore.Value +
                        group.FeelsHeOrSheIsBad.Value +
                        group.TakesUnnecessaryRisks.Value +
                        group.GetsHurtFrequently.Value +
                        group.ActsYoungerThanChildrenHisOrHerAge.Value +
                        group.DoesNoShowFeelings.Value;
            return int.Parse(total.ToString());
        }
    }
}