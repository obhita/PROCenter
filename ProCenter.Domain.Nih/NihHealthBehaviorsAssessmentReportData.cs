// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

namespace ProCenter.Domain.Nih
{
    #region Using Statements

    using System;
    using System.Text;

    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    #region Using Statements

    #endregion

    /// <summary>
    /// NIDA Report Data.
    /// </summary>
    public class NihHealthBehaviorsAssessmentReportData
    {
        #region Fields

        private readonly NihHealthBehaviorsAssessment _nihHealthBehaviorsAssessmentAssessment;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentReportData" /> class.
        /// </summary>
        public NihHealthBehaviorsAssessmentReportData ()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentReportData" /> class.
        /// </summary>
        /// <param name="healthBehaviorsAssessmentAssessment">The healthBehaviorsAssessmentAssessment.</param>
        public NihHealthBehaviorsAssessmentReportData ( NihHealthBehaviorsAssessment healthBehaviorsAssessmentAssessment )
        {
            _nihHealthBehaviorsAssessmentAssessment = healthBehaviorsAssessmentAssessment;
            SetDiet ();
            SetWeight ();
            SetExercise ();
            SetStress ();
            SetAnxiety ();
            SetDepression ();
            SetSleep ();
            SetTobacco ();
            SetAlcohol ();
            SetDrugUse ();
            SetGeneralHealth ();
            SetDietRecommendation ();
            SetAlcoholRecommendation ();
            SetWeightRecommendation ();
            SetSleepRecommendation ();
            SetStressRecommendation ();
            SetExerciseRecommendation ();
            SetDrugUseRecommendation ();
            SetAnxietyRecommendation ();
            SetDepressionRecommendation ();
            SetTobaccoRecommendation ();
            SetHeadersAndLabels();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the alcohol label.
        /// </summary>
        /// <value>
        /// The alcohol label.
        /// </value>
        public string AlcoholLabel { get; private set; }

        /// <summary>
        /// Gets the anxiety label.
        /// </summary>
        /// <value>
        /// The anxiety label.
        /// </value>
        public string AnxietyLabel { get; private set; }

        /// <summary>
        /// Gets the depression label.
        /// </summary>
        /// <value>
        /// The depression label.
        /// </value>
        public string DepressionLabel { get; private set; }

        /// <summary>
        /// Gets the diet label.
        /// </summary>
        /// <value>
        /// The diet label.
        /// </value>
        public string DietLabel { get; private set; }

        /// <summary>
        /// Gets the drug use label.
        /// </summary>
        /// <value>
        /// The drug use label.
        /// </value>
        public string DrugUseLabel { get; private set; }

        /// <summary>
        /// Gets the exercise label.
        /// </summary>
        /// <value>
        /// The exercise label.
        /// </value>
        public string ExerciseLabel { get; private set; }

        /// <summary>
        /// Gets the general health label.
        /// </summary>
        /// <value>
        /// The general health label.
        /// </value>
        public string GeneralHealthLabel { get; private set; }

        /// <summary>
        /// Gets the sleep label.
        /// </summary>
        /// <value>
        /// The sleep label.
        /// </value>
        public string SleepLabel { get; private set; }

        /// <summary>
        /// Gets the stress label.
        /// </summary>
        /// <value>
        /// The stress label.
        /// </value>
        public string StressLabel { get; private set; }

        /// <summary>
        /// Gets the tobacco label.
        /// </summary>
        /// <value>
        /// The tobacco label.
        /// </value>
        public string TobaccoLabel { get; private set; }

        /// <summary>
        /// Gets the weight label.
        /// </summary>
        /// <value>
        /// The weight label.
        /// </value>
        public string WeightLabel { get; private set; }

        /// <summary>
        /// Gets the alcohol summary label.
        /// </summary>
        /// <value>
        /// The alcohol summary label.
        /// </value>
        public string AlcoholSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the anxiety summary label.
        /// </summary>
        /// <value>
        /// The anxiety summary label.
        /// </value>
        public string AnxietySummaryLabel { get; private set; }

        /// <summary>
        /// Gets the depression summary label.
        /// </summary>
        /// <value>
        /// The depression summary label.
        /// </value>
        public string DepressionSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the diet summary label.
        /// </summary>
        /// <value>
        /// The diet summary label.
        /// </value>
        public string DietSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the drug use summary label.
        /// </summary>
        /// <value>
        /// The drug use summary label.
        /// </value>
        public string DrugUseSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the exercise summary label.
        /// </summary>
        /// <value>
        /// The exercise summary label.
        /// </value>
        public string ExerciseSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the general health summary label.
        /// </summary>
        /// <value>
        /// The general health summary label.
        /// </value>
        public string GeneralHealthSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the sleep summary label.
        /// </summary>
        /// <value>
        /// The sleep summary label.
        /// </value>
        public string SleepSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the stress summary label.
        /// </summary>
        /// <value>
        /// The stress summary label.
        /// </value>
        public string StressSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the tobacco summary label.
        /// </summary>
        /// <value>
        /// The tobacco summary label.
        /// </value>
        public string TobaccoSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the weight summary label.
        /// </summary>
        /// <value>
        /// The weight summary label.
        /// </value>
        public string WeightSummaryLabel { get; private set; }

        /// <summary>
        /// Gets the check for accuracy.
        /// </summary>
        /// <value>
        /// The check for accuracy.
        /// </value>
        public string CheckForAccuracy { get; private set; }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary { get; private set; }

        /// <summary>
        /// Gets the summary results label.
        /// </summary>
        /// <value>
        /// The summary results label.
        /// </value>
        public string SummaryResultsLabel { get; private set; }

        /// <summary>
        /// Gets the dear.
        /// </summary>
        /// <value>
        /// The dear.
        /// </value>
        public string Dear { get; private set; }

        /// <summary>
        /// Gets the summary header.
        /// </summary>
        /// <value>
        /// The summary header.
        /// </value>
        public string SummaryHeader { get; private set; }

        /// <summary>
        ///     Gets the recommendations for topics header.
        /// </summary>
        public string HeaderRecommendationsForTopics { get; private set; }

        /// <summary>
        ///     Gets or sets the label health care provider recommendations.
        /// </summary>
        /// <value>
        ///     The label health care provider recommendations.
        /// </value>
        public string LabelHealthCareProviderRecommendations { get; set; }

        /// <summary>
        ///     Gets the alcohol.
        /// </summary>
        /// <value>
        ///     The alcohol.
        /// </value>
        public string Alcohol { get; private set; }

        /// <summary>
        ///     Gets the alcohol recommondation.
        /// </summary>
        /// <value>
        ///     The alcohol recommondation.
        /// </value>
        public string AlcoholRecommendation { get; private set; }

        /// <summary>
        ///     Gets the anxiety.
        /// </summary>
        /// <value>
        ///     The anxiety.
        /// </value>
        public string Anxiety { get; private set; }

        /// <summary>
        ///     Gets the anxiet recommondationy.
        /// </summary>
        /// <value>
        ///     The anxiet recommondationy.
        /// </value>
        public string AnxietyRecommendation { get; private set; }

        /// <summary>
        ///     Gets the depression.
        /// </summary>
        /// <value>
        ///     The depression.
        /// </value>
        public string Depression { get; private set; }

        /// <summary>
        ///     Gets the depression recommondation.
        /// </summary>
        /// <value>
        ///     The depression recommondation.
        /// </value>
        public string DepressionRecommendation { get; private set; }

        /// <summary>
        ///     Gets the diet.
        /// </summary>
        /// <value>
        ///     The diet.
        /// </value>
        public string Diet { get; private set; }

        /// <summary>
        ///     Gets the diet recommondation.
        /// </summary>
        /// <value>
        ///     The diet recommondation.
        /// </value>
        public string DietRecommendation { get; private set; }

        /// <summary>
        ///     Gets the drug use.
        /// </summary>
        /// <value>
        ///     The drug use.
        /// </value>
        public string DrugUse { get; private set; }

        /// <summary>
        ///     Gets the drug use recommondation.
        /// </summary>
        /// <value>
        ///     The drug use recommondation.
        /// </value>
        public string DrugUseRecommendation { get; private set; }

        /// <summary>
        ///     Gets the excercise.
        /// </summary>
        /// <value>
        ///     The excercise.
        /// </value>
        public string Excercise { get; private set; }

        /// <summary>
        ///     Gets the excercise recommondation.
        /// </summary>
        /// <value>
        ///     The excercise recommondation.
        /// </value>
        public string ExcerciseRecommendation { get; private set; }

        /// <summary>
        ///     Gets the general health.
        /// </summary>
        /// <value>
        ///     The general health.
        /// </value>
        public string GeneralHealth { get; private set; }

        /// <summary>
        ///     Gets the sleep.
        /// </summary>
        /// <value>
        ///     The sleep.
        /// </value>
        public string Sleep { get; private set; }

        /// <summary>
        ///     Gets the sleep recommondation.
        /// </summary>
        /// <value>
        ///     The sleep recommondation.
        /// </value>
        public string SleepRecommendation { get; private set; }

        /// <summary>
        ///     Gets the stress.
        /// </summary>
        /// <value>
        ///     The stress.
        /// </value>
        public string Stress { get; private set; }

        /// <summary>
        ///     Gets the stress recommondation.
        /// </summary>
        /// <value>
        ///     The stress recommondation.
        /// </value>
        public string StressRecommendation { get; private set; }

        /// <summary>
        ///     Gets or sets the summary report information.
        /// </summary>
        /// <value>
        ///     The summary report information.
        /// </value>
        public SummaryReportInfo SummaryReportInfo { get; set; }

        /// <summary>
        ///     Gets the tobacco.
        /// </summary>
        /// <value>
        ///     The tobacco.
        /// </value>
        public string Tobacco { get; private set; }

        /// <summary>
        ///     Gets the tobacco recommondation.
        /// </summary>
        /// <value>
        ///     The tobacco recommondation.
        /// </value>
        public string TobaccoRecommendation { get; private set; }

        /// <summary>
        ///     Gets the weight.
        /// </summary>
        /// <value>
        ///     The weight.
        /// </value>
        public string Weight { get; private set; }

        /// <summary>
        ///     Gets the weight recommondation.
        /// </summary>
        /// <value>
        ///     The weight recommondation.
        /// </value>
        public string WeightRecommendation { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Converts the frequency to string.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns>Returns frequency's value.</returns>
        private string ConvertFrequencyToString ( Lookup lookup )
        {
            return lookup.DisplayName.Replace(" " + NihHealthBehaviorsAssessmentReportStrings.Times, string.Empty)
                .Replace(" " + NihHealthBehaviorsAssessmentReportStrings.Time, string.Empty);
        }

        /// <summary>
        ///     Sets the alcohol.
        /// </summary>
        private void SetAlcohol ()
        {
            Alcohol = string.Format(NihHealthBehaviorsAssessmentReportStrings.AlcoholReported, 
                _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouHadFourToFiveOrMoreDrinksInADay.DisplayName.ToLower());
        }

        /// <summary>
        ///     Sets the alcohol recommendation.
        /// </summary>
        private void SetAlcoholRecommendation ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouHadFourToFiveOrMoreDrinksInADay.CodedConcept.Code.Equals( 
                AlcoholDrugUseFrequency.OneToThreeTimes.CodedConcept.Code ) )
            {
                AlcoholRecommendation = NihHealthBehaviorsAssessmentReportStrings.AlcoholRecommendationOneToThreeTimes;
            }

            if ( _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouHadFourToFiveOrMoreDrinksInADay.Value.Equals( 
                AlcoholDrugUseFrequency.FourOrMoreTimes.Value ) )
            {
                AlcoholRecommendation = NihHealthBehaviorsAssessmentReportStrings.AlcoholRecommendationFourOrMoreTimes;
            }
        }

        /// <summary>
        ///     Sets the anxiety.
        /// </summary>
        private void SetAnxiety ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingNervousAnxious.CodedConcept.Code == DaysFrequency.NotAtAll.CodedConcept.Code )
            {
                Anxiety = NihHealthBehaviorsAssessmentReportStrings.NoStressOrAnxious;
            }
            else
            {
                Anxiety = NihHealthBehaviorsAssessmentReportStrings.NervousOrAnxious + " " 
                    + _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingNervousAnxious.DisplayName.ToLower ();
            }
            if ( _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.NotBeingAbleToStopWorrying.CodedConcept.Code == DaysFrequency.NotAtAll.CodedConcept.Code )
            {
                Anxiety += " " + NihHealthBehaviorsAssessmentReportStrings.StopOrControlWorry;
            }
            else
            {
                Anxiety += " " + NihHealthBehaviorsAssessmentReportStrings.NotAbleToStopOrControlWorry + " " 
                    + _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.NotBeingAbleToStopWorrying.DisplayName.ToLower ();
            }
            Anxiety += " " + NihHealthBehaviorsAssessmentReportStrings.Past2Weeks;
        }

        /// <summary>
        ///     Sets the anxiety recommendation.
        /// </summary>
        private void SetAnxietyRecommendation ()
        {
            var anxietyFlag = _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingNervousAnxious.Value +
                              _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.NotBeingAbleToStopWorrying.Value;

            if ( anxietyFlag >= 4 )
            {
                AnxietyRecommendation = NihHealthBehaviorsAssessmentReportStrings.AnxietyRecommendation;
            }
        }

        /// <summary>
        ///     Sets the depression.
        /// </summary>
        private void SetDepression ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingDownDepressedOrHopeless.CodedConcept.Code == DaysFrequency.NotAtAll.CodedConcept.Code )
            {
                Depression = NihHealthBehaviorsAssessmentReportStrings.NotDepressedOrDown;
            }
            else
            {
                Depression = NihHealthBehaviorsAssessmentReportStrings.DepressedOrDown + " " +
                             _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingDownDepressedOrHopeless.DisplayName.ToLower ();
            }
            if ( _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.LittleInterestOrPleasureInDoingThings.CodedConcept.Code == DaysFrequency.NotAtAll.CodedConcept.Code )
            {
                Depression += " " + NihHealthBehaviorsAssessmentReportStrings.OrLittleInterestInDoingThings;
            }
            else
            {
                Depression += " " + NihHealthBehaviorsAssessmentReportStrings.AndLittleInterestInDoingThings + " " +
                              _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.LittleInterestOrPleasureInDoingThings.DisplayName.ToLower ();
            }
            Depression += " " + NihHealthBehaviorsAssessmentReportStrings.Past2Weeks;
        }

        /// <summary>
        ///     Sets the depression recommendation.
        /// </summary>
        private void SetDepressionRecommendation ()
        {
            var depresssionFlag = _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.FeelingDownDepressedOrHopeless.Value +
                                  _nihHealthBehaviorsAssessmentAssessment.ProblemGroup.LittleInterestOrPleasureInDoingThings.Value;

            if ( depresssionFlag >= 4 )
            {
                DepressionRecommendation = NihHealthBehaviorsAssessmentReportStrings.DepressionRecommendation;
            }
        }

        /// <summary>
        ///     Sets the diet.
        /// </summary>
        private void SetDiet ()
        {
            Diet = string.Format (
                NihHealthBehaviorsAssessmentReportStrings.DietReported,
                ConvertFrequencyToString ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYouEatFastFoodOrSnacks ),
                ConvertFrequencyToString ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYoueatFruitsOrVegetables ),
                ConvertFrequencyToString ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManySodAndSugarDrinks ) );
        }

        /// <summary>
        ///     Sets the diet recommendation.
        /// </summary>
        private void SetDietRecommendation ()
        {
            var dietFlag = 0;
            var recommendation = new StringBuilder ();

            SetFastFoodRecommendation ( recommendation, ref dietFlag );
            SetFruitsVegetablesRecommendation ( recommendation, ref dietFlag );
            SetSodaAndSugarDrinksRecommendation ( recommendation, ref dietFlag );

            if ( dietFlag >= 2 )
            {
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietRecommendationSummary + " ");
            }
            DietRecommendation = recommendation.ToString ();
        }

        /// <summary>
        ///     Sets the drug use.
        /// </summary>
        private void SetDrugUse ()
        {
            DrugUse =
                string.Format (
                    _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouUsedIllegalDrugsForNonMedicalReasons.CodedConcept.Code.Equals( 
                        AlcoholDrugUseFrequency.Never.CodedConcept.Code )
                        ? NihHealthBehaviorsAssessmentReportStrings.DrugUseReportedNever
                        : NihHealthBehaviorsAssessmentReportStrings.DrugUseReportedOther,
                    _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouUsedIllegalDrugsForNonMedicalReasons.Value );
        }

        /// <summary>
        ///     Sets the drug use recommendation.
        /// </summary>
        private void SetDrugUseRecommendation ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouUsedIllegalDrugsForNonMedicalReasons.CodedConcept.Code
                .Equals ( AlcoholDrugUseFrequency.OneToThreeTimes.CodedConcept.Code ) )
            {
                DrugUseRecommendation = NihHealthBehaviorsAssessmentReportStrings.DrugUseRecommendationOneToThreeTimes;
            }
            else if ( _nihHealthBehaviorsAssessmentAssessment.HowManyTimesHaveYouUsedIllegalDrugsForNonMedicalReasons.CodedConcept.Code
                .Equals ( AlcoholDrugUseFrequency.FourOrMoreTimes.CodedConcept.Code ) )
            {
                DrugUseRecommendation = NihHealthBehaviorsAssessmentReportStrings.DrugUseRecommendationFourOrMoreTimes;
            }
        }

        /// <summary>
        ///     Sets the exercise.
        /// </summary>
        private void SetExercise ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyMinutesOnAverageDoYouExcercise <= 0 )
            {
                Excercise = NihHealthBehaviorsAssessmentReportStrings.NoExercise;
            }
            else
            {
                var recommend = string.Empty;
                var totalMinutes = _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyMinutesOnAverageDoYouExcercise 
                    * _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyDaysModerateToStrenuousExcercise;
                if ( totalMinutes <= 145 )
                {
                    recommend = NihHealthBehaviorsAssessmentReportStrings.Exercise150;
                }
                else if ( totalMinutes > 145 )
                {
                    recommend = NihHealthBehaviorsAssessmentReportStrings.Exercise200;
                }
                Excercise = string.Format (
                    NihHealthBehaviorsAssessmentReportStrings.ExerciseSummary,
                    _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyMinutesOnAverageDoYouExcercise,
                    _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyDaysModerateToStrenuousExcercise ) + " " + recommend;
            }
        }

        /// <summary>
        ///     Sets the exercise recommendation.
        /// </summary>
        private void SetExerciseRecommendation ()
        {
            var totalMins = _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyMinutesOnAverageDoYouExcercise *
                            _nihHealthBehaviorsAssessmentAssessment.ExerciseGroup.HowManyDaysModerateToStrenuousExcercise;

            if ( totalMins >= 0 && totalMins <= 145 )
            {
                ExcerciseRecommendation = NihHealthBehaviorsAssessmentReportStrings.Exercise150;
            }
            else if ( totalMins >= 146 && totalMins <= 300 )
            {
                ExcerciseRecommendation = NihHealthBehaviorsAssessmentReportStrings.Exercise200;
            }
        }

        /// <summary>
        ///     Sets the fast food recommendation.
        /// </summary>
        /// <param name="recommendation">The recommendation.</param>
        /// <param name="dietFlag">The diet flag.</param>
        private void SetFastFoodRecommendation ( StringBuilder recommendation, ref int dietFlag )
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYouEatFastFoodOrSnacks.CodedConcept.Code.Equals ( 
                FastFoodFrequency.LessThanOneTime.CodedConcept.Code ) )
            {
                recommendation.Append ( NihHealthBehaviorsAssessmentReportStrings.DietFastFoodRecommendationLessThanOneTime + " ");
            }

            if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYouEatFastFoodOrSnacks.CodedConcept.Code.Equals ( 
                FastFoodFrequency.FourOrMoreTimes.CodedConcept.Code ) )
            {
                dietFlag += 1;
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietFastFoodRecommendationFourOrMoreTimes + " ");
            }
        }

        /// <summary>
        ///     Sets the fruits vegetables recommendation.
        /// </summary>
        /// <param name="recommendation">The recommendation.</param>
        /// <param name="dietFlag">The diet flag.</param>
        private void SetFruitsVegetablesRecommendation ( StringBuilder recommendation, ref int dietFlag )
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYoueatFruitsOrVegetables.CodedConcept.Code.Equals ( 
                FruitsVegetablesFrequency.FiveOrMore.CodedConcept.Code ) )
            {
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietFruitsVegetablesRecommendationFiveOrMoreTimes + " ");
            }

            if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManyTimesDidYoueatFruitsOrVegetables.CodedConcept.Code.Equals ( 
                FruitsVegetablesFrequency.TwoOrLess.CodedConcept.Code ) )
            {
                dietFlag += 1;
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietFruitsVegetablesRecommendationTwoOrLessTimes + " ");
            }
        }

        /// <summary>
        ///     Sets the general health.
        /// </summary>
        private void SetGeneralHealth ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.InGeneralWouldYouSayYourHealthIs.CodedConcept.Code.Equals ( 
                HealthCondition.Poor.CodedConcept.Code ) )
            {
                GeneralHealth = NihHealthBehaviorsAssessmentReportStrings.PoorHealthFactors + " " 
                    + _nihHealthBehaviorsAssessmentAssessment.PoorHealthFactors;
            }
            else
            {
                GeneralHealth = string.Format ( NihHealthBehaviorsAssessmentReportStrings.GeneralHealthReported, 
                    _nihHealthBehaviorsAssessmentAssessment.InGeneralWouldYouSayYourHealthIs.DisplayName.ToLower () );
            }
        }

        /// <summary>
        ///     Sets the headers and labels.
        /// </summary>
        private void SetHeadersAndLabels ()
        {
            DietSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.DietLabel;
            WeightSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.WeightLabel;
            ExerciseSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.ExerciseLabel;
            StressSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.StressLabel;
            AnxietySummaryLabel = NihHealthBehaviorsAssessmentReportStrings.AnxietyLabel;
            DepressionSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.DepressionLabel;
            SleepSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.SleepLabel;
            TobaccoSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.TobaccoLabel;
            AlcoholSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.AlcoholLabel;
            DrugUseSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.DrugUseLabel;
            GeneralHealthSummaryLabel = NihHealthBehaviorsAssessmentReportStrings.GeneralHealthLabel;

            DietLabel = !string.IsNullOrEmpty(DietRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.DietLabel : null;
            WeightLabel = !string.IsNullOrEmpty(WeightRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.WeightLabel : null;
            ExerciseLabel = !string.IsNullOrEmpty(ExcerciseRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.ExerciseLabel : null;
            StressLabel = !string.IsNullOrEmpty(StressRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.StressLabel : null;
            AnxietyLabel = !string.IsNullOrEmpty(AnxietyRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.AnxietyLabel : null;
            DepressionLabel = !string.IsNullOrEmpty(DepressionRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.DepressionLabel : null;
            SleepLabel = !string.IsNullOrEmpty(SleepRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.SleepLabel : null;
            TobaccoLabel = !string.IsNullOrEmpty(TobaccoRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.TobaccoLabel : null;
            AlcoholLabel = !string.IsNullOrEmpty(AlcoholRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.AlcoholLabel : null;
            DrugUseLabel = !string.IsNullOrEmpty(DrugUseRecommendation) ? NihHealthBehaviorsAssessmentReportStrings.DrugUseLabel : null;

            GeneralHealthLabel = NihHealthBehaviorsAssessmentReportStrings.GeneralHealthLabel;
            CheckForAccuracy = NihHealthBehaviorsAssessmentReportStrings.CheckForAccuracy;
            Summary = NihHealthBehaviorsAssessmentReportStrings.Summary;
            SummaryResultsLabel = NihHealthBehaviorsAssessmentReportStrings.SummaryResultsFor;
            Dear = NihHealthBehaviorsAssessmentReportStrings.Dear;
            SummaryHeader = NihHealthBehaviorsAssessmentReportStrings.SummaryHeader;
            HeaderRecommendationsForTopics = NihHealthBehaviorsAssessmentReportStrings.Header_RecommendationsForTopics;
            LabelHealthCareProviderRecommendations = NihHealthBehaviorsAssessmentReportStrings.Label_HealthCareProviderRecommendations;
        }

        /// <summary>
        ///     Sets the sleep.
        /// </summary>
        private void SetSleep ()
        {
            Sleep = _nihHealthBehaviorsAssessmentAssessment.DoYouSnoreOrHasAnybodyToldYou ? NihHealthBehaviorsAssessmentReportStrings.YouDoSnore : 
                NihHealthBehaviorsAssessmentReportStrings.YouDoNotSnore;
            if ( _nihHealthBehaviorsAssessmentAssessment.HowOftenSleepDuringTheDay.CodedConcept.Code == SleepDuringDayFrequency.Never.CodedConcept.Code )
            {
                Sleep += " " + NihHealthBehaviorsAssessmentReportStrings.AndNeverFallAsleepDuringDay;
            }
            else
            {
                Sleep += " " + string.Format ( NihHealthBehaviorsAssessmentReportStrings.AndFallAsleepDuringDay, 
                    _nihHealthBehaviorsAssessmentAssessment.HowOftenSleepDuringTheDay.DisplayName.ToLower () );
            }
            Sleep += " " + NihHealthBehaviorsAssessmentReportStrings.InThePastWeek;
        }

        /// <summary>
        ///     Sets the sleep recommendation.
        /// </summary>
        private void SetSleepRecommendation ()
        {
            SleepRecommendation = _nihHealthBehaviorsAssessmentAssessment.HowOftenSleepDuringTheDay.CodedConcept.Code.Equals ( 
                SleepDuringDayFrequency.Always.CodedConcept.Code )
                ? NihHealthBehaviorsAssessmentReportStrings.ThisIsAConcern
                : null;
        }

        /// <summary>
        ///     Sets the soda and sugar drinks recommendation.
        /// </summary>
        /// <param name="recommendation">The recommendation.</param>
        /// <param name="dietFlag">The diet flag.</param>
        private void SetSodaAndSugarDrinksRecommendation ( StringBuilder recommendation, ref int dietFlag )
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManySodAndSugarDrinks.CodedConcept.Code.Equals ( 
                SodaSugarFrequency.LessThanOneTime.CodedConcept.Code ) )
            {
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietSodaSugarRecommendationLessThanOne + " ");
            }
            else if ( _nihHealthBehaviorsAssessmentAssessment.DietGroup.HowManySodAndSugarDrinks.CodedConcept.Code.Equals ( 
                SodaSugarFrequency.ThreeOrMoreTimes.CodedConcept.Code ) )
            {
                dietFlag += 1;
                recommendation.Append(NihHealthBehaviorsAssessmentReportStrings.DietSodaSugarRecommendationThreeOrMore + " ");
            }
        }

        /// <summary>
        ///     Sets the stress.
        /// </summary>
        private void SetStress ()
        {
            var level = NihHealthBehaviorsAssessmentReportStrings.StressLow;
            var stressNumber = _nihHealthBehaviorsAssessmentAssessment.HowMuchStressInLast7Days;
            if ( stressNumber >= 0 && stressNumber <= 3 )
            {
                level = NihHealthBehaviorsAssessmentReportStrings.StressLow;
            }
            else if ( stressNumber >= 4 && stressNumber <= 6 )
            {
                level = NihHealthBehaviorsAssessmentReportStrings.StrressModerate;
            }
            else if ( stressNumber >= 7 && stressNumber <= 10 )
            {
                level = NihHealthBehaviorsAssessmentReportStrings.StressHigh;
            }
            Stress = string.Format ( NihHealthBehaviorsAssessmentReportStrings.StressReported, level, stressNumber );
        }

        /// <summary>
        ///     Sets the stress recommendation.
        /// </summary>
        private void SetStressRecommendation ()
        {
            if ( _nihHealthBehaviorsAssessmentAssessment.HowMuchStressInLast7Days >= 5 )
            {
                StressRecommendation = NihHealthBehaviorsAssessmentReportStrings.ThisIsAConcern;
            }
        }

        /// <summary>
        ///     Sets the tobacco.
        /// </summary>
        private void SetTobacco ()
        {
            if ( !_nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmoked && 
                !_nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmokelessTobacco )
            {
                Tobacco = NihHealthBehaviorsAssessmentReportStrings.YouDoNotUseTobacco;
                return;
            }
            if ( _nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmoked )
            {
                Tobacco += NihHealthBehaviorsAssessmentReportStrings.YouHaveSmoked;
            }
            if ( _nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmokelessTobacco )
            {
                if ( string.IsNullOrWhiteSpace ( Tobacco ) )
                {
                    Tobacco = NihHealthBehaviorsAssessmentReportStrings.YouHaveUsedSmokelessTobacco;
                }
                else
                {
                    Tobacco += " " + NihHealthBehaviorsAssessmentReportStrings.AndYouHaveUseedSmokelessTobacco;
                }
            }
            Tobacco += " " + NihHealthBehaviorsAssessmentReportStrings.InThePast30Days;
        }

        /// <summary>
        ///     Sets the tobacco recommendation.
        /// </summary>
        private void SetTobaccoRecommendation ()
        {
            var tobaccoFlag = 0;
            if ( _nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmoked )
            {
                tobaccoFlag += 1;
            }
            if ( _nihHealthBehaviorsAssessmentAssessment.SmokeGroup.HaveUsedTobaccoInLast30DaysSmokelessTobacco )
            {
                tobaccoFlag += 1;
            }
            if ( tobaccoFlag >= 1 )
            {
                // todo: need to confirm the requirements
                TobaccoRecommendation = string.Empty;
            }
        }

        /// <summary>
        ///     Sets the weight.
        /// </summary>
        private void SetWeight ()
        {
            var feet = _nihHealthBehaviorsAssessmentAssessment.WhatIsYourHeight / 12;
            var inches = _nihHealthBehaviorsAssessmentAssessment.WhatIsYourHeight % 12;
            Weight = string.Format ( 
                NihHealthBehaviorsAssessmentReportStrings.WeightHeight, 
                _nihHealthBehaviorsAssessmentAssessment.WhatIsYourWeight, 
                feet + "'" + inches + "\"" );
        }

        /// <summary>
        ///     Sets the weight recommendation.
        /// </summary>
        private void SetWeightRecommendation ()
        {
            var nih = _nihHealthBehaviorsAssessmentAssessment;
            var bmi = Math.Round (
                ((double)nih.WhatIsYourWeight / (nih.WhatIsYourHeight * nih.WhatIsYourHeight) ) * 703.06958, 1 );

            if ( bmi < 15 )
            {
                // very severely underweight bmi < 15
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.FollowUpWithDoctor;
            }
            else if ( bmi < 16 )
            {
                // severely underweight bmi >= 15 && bmi < 16
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.FollowUpWithDoctor;
            }
            else if ( bmi < 18.5 )
            {
                // underweight bmi >= 16 && bmi < 18.5
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.FollowUpWithDoctor;
            }
            else if ( bmi < 25 )
            {
                // normal bmi >= 18.5 && bmi < 25
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.WeightRecommendationNormal;
            }
            else if ( bmi < 30 )
            {
                // overweight bmi >= 25 && bmi < 30
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.WeightRecommendationOverweight;
            }
            else if ( bmi < 35 )
            {
                // moderately obese bmi >= 30 && bmi < 35
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.ObesityWarning;
            }
            else if ( bmi < 40 )
            {
                // severely obese bmi >= 35 && bmi < 40
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.ObesityWarning;
            }
            else
            {
                // very severely obese bmi > 40
                WeightRecommendation = NihHealthBehaviorsAssessmentReportStrings.ObesityWarning;
            }
        }

        #endregion
    }
}