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

    /// <summary>The Youth Pediatric Sympton Checklist report engine class.</summary>
    [ReportEngine(typeof(YouthPediatricSymptonChecklist))]
    public class YouthPediatricSymptonChecklistSummaryReport : IReportEngine
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IStaffRepository _staffRepository;

        private YouthPediatricSymptonChecklistReportData _youthPediatricSymptonChecklistReportData;

        private YouthPediatricSymptonChecklist _youthPediatricSymptonChecklist;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="YouthPediatricSymptonChecklistSummaryReport"/> class.
        /// </summary>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="staffRepository">The staff repository.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        public YouthPediatricSymptonChecklistSummaryReport(
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
            _youthPediatricSymptonChecklist = new YouthPediatricSymptonChecklist(assessment);

            _youthPediatricSymptonChecklistReportData = new YouthPediatricSymptonChecklistReportData(_youthPediatricSymptonChecklist);
            PopulateReportData (patient, staff );
            var reportDataCollection = new PediatricSymptonChecklistReportDataCollection
                                       {
                                           _youthPediatricSymptonChecklistReportData
                                       };
            var report = new YouthPediatricSymptonChecklistReport
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
            var group = _youthPediatricSymptonChecklistReportData.YouthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup;

            _youthPediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal = GetAttentionProblemSubscaleTotal();
            _youthPediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal = GetAnxietyDepressionSubscaleTotal();
            _youthPediatricSymptonChecklistReportData.ConductProblemSubscaleTotal = GetConductProblemSubscaleTotal();
            _youthPediatricSymptonChecklistReportData.TotalScore = _youthPediatricSymptonChecklistReportData.AttentionProblemSubscaleTotal +
                         _youthPediatricSymptonChecklistReportData.AnxietyDepressionSubscaleTotal +
                         _youthPediatricSymptonChecklistReportData.ConductProblemSubscaleTotal +
                         GetOtherIssuesTotal();

            var values = new List<TimeFrequency>
                             {
                                 _youthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup.HasTroubleWithTeacher,
                                 _youthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup.LessInterestedInSchool,
                                 _youthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup.AbsentFromSchool,
                                 _youthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup.SchoolGradesDropping
                             };
            _youthPediatricSymptonChecklistReportData.TotalScore -= Common.AdjustScoreBasedOnAge(patient, values);

            _youthPediatricSymptonChecklistReportData.Age = Common.GetAge(patient);
            _youthPediatricSymptonChecklistReportData.StaffName = staff.Name.FullName;
            _youthPediatricSymptonChecklistReportData.ScreeningDate = _youthPediatricSymptonChecklist.AssessmentInstance.CreatedDate.ToLongDateString();

            _youthPediatricSymptonChecklistReportData.Answer4 = Common.GetAnswerValue(group.FidgetyUnableToSitStill);
            _youthPediatricSymptonChecklistReportData.Answer7 = Common.GetAnswerValue(group.ActsAsIfDrivenByAMotor);
            _youthPediatricSymptonChecklistReportData.Answer8 = Common.GetAnswerValue(group.DaydreamsTooMuch);
            _youthPediatricSymptonChecklistReportData.Answer9 = Common.GetAnswerValue(group.DistractedEasily);
            _youthPediatricSymptonChecklistReportData.Answer14 = Common.GetAnswerValue(group.HasTroubleConcentrating);

            _youthPediatricSymptonChecklistReportData.Answer11 = Common.GetAnswerValue(group.FeelsSadUnhappy);
            _youthPediatricSymptonChecklistReportData.Answer13 = Common.GetAnswerValue(group.FeelsHopeless);
            _youthPediatricSymptonChecklistReportData.Answer19 = Common.GetAnswerValue(group.IsDownOnHimOrHerself);
            _youthPediatricSymptonChecklistReportData.Answer22 = Common.GetAnswerValue(group.WorriesAlot);
            _youthPediatricSymptonChecklistReportData.Answer27 = Common.GetAnswerValue(group.SeemsToBeHavingLessFun);

            _youthPediatricSymptonChecklistReportData.Answer16 = Common.GetAnswerValue(group.FightsWithOtherChildren);
            _youthPediatricSymptonChecklistReportData.Answer29 = Common.GetAnswerValue(group.DoesNotListenToRules);
            _youthPediatricSymptonChecklistReportData.Answer31 = Common.GetAnswerValue(group.DoesNotUnderstandOtherPeoplesFeelings);
            _youthPediatricSymptonChecklistReportData.Answer32 = Common.GetAnswerValue(group.TeasesOthers);
            _youthPediatricSymptonChecklistReportData.Answer33 = Common.GetAnswerValue(group.BlamesOthersForHisOrHerTroubles);
            _youthPediatricSymptonChecklistReportData.Answer34 = Common.GetAnswerValue(group.TakesThingsThatDoNotBelongToHimOrHer);
            _youthPediatricSymptonChecklistReportData.Answer35 = Common.GetAnswerValue(group.RefusesToShare);

            _youthPediatricSymptonChecklistReportData.Answer1 = Common.GetAnswerValue(group.ComplainsOfAchesAndPains);
            _youthPediatricSymptonChecklistReportData.Answer2 = Common.GetAnswerValue(group.SpendsMoreTimeAlone);
            _youthPediatricSymptonChecklistReportData.Answer3 = Common.GetAnswerValue(group.TiresEasily);

            // if 4-5 years old then this should say N/A
            if ( _youthPediatricSymptonChecklistReportData.Age >= 4 && _youthPediatricSymptonChecklistReportData.Age <= 5 )
            {
                _youthPediatricSymptonChecklistReportData.Answer5 = "0 - N/A";
                _youthPediatricSymptonChecklistReportData.Answer6 = "0 - N/A";
                _youthPediatricSymptonChecklistReportData.Answer17 = "0 - N/A";
                _youthPediatricSymptonChecklistReportData.Answer18 = "0 - N/A";
            }
            else
            {
                _youthPediatricSymptonChecklistReportData.Answer5 = Common.GetAnswerValue(group.HasTroubleWithTeacher);
                _youthPediatricSymptonChecklistReportData.Answer6 = Common.GetAnswerValue(group.LessInterestedInSchool);
                _youthPediatricSymptonChecklistReportData.Answer17 = Common.GetAnswerValue(group.AbsentFromSchool);
                _youthPediatricSymptonChecklistReportData.Answer18 = Common.GetAnswerValue(group.SchoolGradesDropping);
            }

            _youthPediatricSymptonChecklistReportData.Answer10 = Common.GetAnswerValue(group.IsAfraidOfNewSituations);
            _youthPediatricSymptonChecklistReportData.Answer12 = Common.GetAnswerValue(group.IsIrritableAngry);
            _youthPediatricSymptonChecklistReportData.Answer15 = Common.GetAnswerValue(group.LessInterestedInFriends);

            _youthPediatricSymptonChecklistReportData.Answer20 = Common.GetAnswerValue(group.VisitsTheDoctorWithDoctorFindingNothingWrong);
            _youthPediatricSymptonChecklistReportData.Answer21 = Common.GetAnswerValue(group.HasTroubleSleeping);
            _youthPediatricSymptonChecklistReportData.Answer23 = Common.GetAnswerValue(group.WantsToBeWithYouMoreThanBefore);
            _youthPediatricSymptonChecklistReportData.Answer24 = Common.GetAnswerValue(group.FeelsHeOrSheIsBad);
            _youthPediatricSymptonChecklistReportData.Answer25 = Common.GetAnswerValue(group.TakesUnnecessaryRisks);
            _youthPediatricSymptonChecklistReportData.Answer26 = Common.GetAnswerValue(group.GetsHurtFrequently);
            _youthPediatricSymptonChecklistReportData.Answer28 = Common.GetAnswerValue(group.ActsYoungerThanChildrenHisOrHerAge);
            _youthPediatricSymptonChecklistReportData.Answer30 = Common.GetAnswerValue(group.DoesNoShowFeelings);
        }

        /// <summary>
        ///     Gets the attention problem subscale total.
        /// </summary>
        /// <returns>Returns an integer of the subtotal.</returns>
        private int GetAttentionProblemSubscaleTotal()
        {
            var group = _youthPediatricSymptonChecklistReportData.YouthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup;
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
            var group = _youthPediatricSymptonChecklistReportData.YouthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup;
            var total = group.FeelsSadUnhappy.Value +
                        group.FeelsHopeless.Value +
                        group.IsDownOnHimOrHerself.Value +
                        group.WorriesAlot.Value +
                        group.SeemsToBeHavingLessFun.Value;
            return int.Parse(total.ToString());
        }

        private int GetConductProblemSubscaleTotal()
        {
            var group = _youthPediatricSymptonChecklistReportData.YouthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup;
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
            var group = _youthPediatricSymptonChecklistReportData.YouthPediatricSymptonChecklist.YouthChildsEmotionalAndBehaviorProblemsGroup;
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