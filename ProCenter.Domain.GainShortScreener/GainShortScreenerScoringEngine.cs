#region License Header

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

#endregion

namespace ProCenter.Domain.GainShortScreener
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Resources;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.PatientModule;

    #endregion

    /// <summary>The gain short screener scoring engine class.</summary>
    public class GainShortScreenerScoringEngine : IScoringEngine
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        private readonly IPatientRepository _patientRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the GainShortScreenerScoringEngine class.
        /// </summary>
        public GainShortScreenerScoringEngine ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GainShortScreenerScoringEngine class.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="patientRepository">The patient repository.</param>
        public GainShortScreenerScoringEngine ( IResourcesManager resourcesManager,
                                                IPatientRepository patientRepository)
        {
            _resourcesManager = resourcesManager;
            _patientRepository = patientRepository;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name of the assessment.
        /// </summary>
        /// <value>
        ///     The name of the assessment.
        /// </value>
        public string AssessmentName
        {
            get { return GainShortScreener.AssessmentCodedConcept.Name; }
        }

        /// <summary>
        ///     Gets the resource manager.
        /// </summary>
        /// <value>
        ///     The resource manager.
        /// </value>
        public ResourceManager ResourceManager
        {
            get { return _resourcesManager.GetResourceManagerByName ( AssessmentName ); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the score.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        public void CalculateScore ( AssessmentInstance assessment )
        {
            var gainShortScreener = new GainShortScreener ( assessment );
            int pastMonth, past90Days, pastYear, lifetime;
            GetInternalDisorderScreenerScore ( gainShortScreener, out pastMonth, out past90Days, out pastYear, out lifetime );
            var internalizingDisorderScore = new GainGroupScore ( pastMonth, past90Days, pastYear, lifetime );
            GetExternalDisorderScreenerScore(gainShortScreener, out pastMonth, out past90Days, out pastYear, out lifetime);
            var externalizingDisorderScore = new GainGroupScore(pastMonth, past90Days, pastYear, lifetime);
            GetSubstanceDisorderScreenerScore(gainShortScreener, out pastMonth, out past90Days, out pastYear, out lifetime);
            var substanceDisorderScore = new GainGroupScore(pastMonth, past90Days, pastYear, lifetime);
            GetCriminalViolenceScreenerScore(gainShortScreener, out pastMonth, out past90Days, out pastYear, out lifetime);
            var criminalViolenceScore = new GainGroupScore(pastMonth, past90Days, pastYear, lifetime);
            var gainShortScreenerScoring = new GainShortScreenerScore ( internalizingDisorderScore, externalizingDisorderScore, substanceDisorderScore, criminalViolenceScore );

            assessment.ScoreComplete(
                    new CodedConcept(CodeSystems.Obhita, string.Empty, string.Empty), 
                    gainShortScreenerScoring, 
                    true);
        }

        #endregion

        #region Methods

        private void GetCriminalViolenceScreenerScore ( GainShortScreener gainShortScreener, out int pastMonth, out int past90Days, out int pastYear, out int lifetime )
        {
            var criminalViolenceGroup = gainShortScreener.TotalDisorderScreenerGroup.CrimeViolenceScreenerGroup;
            var values = new List<LastTimeFrequency>
                         {
                             criminalViolenceGroup.DroveUnderTheInfluenceOfAlcoholOrIllegalDrugs,
                             criminalViolenceGroup.HadADisagreement,
                             criminalViolenceGroup.PurposelyDamagedPropertyThatDidNotBelongToYou,
                             criminalViolenceGroup.SoldDistributedOrHelpedToMakeIllegalDrugs,
                             criminalViolenceGroup.TookSomethingFromAStoreWithoutPaying
                         };
            GetScores(values, out pastMonth, out past90Days, out pastYear, out lifetime);
        }

        private void GetExternalDisorderScreenerScore(GainShortScreener gainShortScreener, out int pastMonth, out int past90Days, out int pastYear, out int lifetime)
        {
            var externalDisorderScreenerGroup = gainShortScreener.TotalDisorderScreenerGroup.ExternalizingDisorderScreenerGroup;
            var values = new List<LastTimeFrequency>
                         {
                             externalDisorderScreenerGroup.HardTimeListening,
                             externalDisorderScreenerGroup.HardTimePayingAttention,
                             externalDisorderScreenerGroup.HardTimeWaitingForYourTurn,
                             externalDisorderScreenerGroup.LiedOrConned,
                             externalDisorderScreenerGroup.StartedPhysicalFights,
                             externalDisorderScreenerGroup.TriedToWinBackGamblingLosses,
                             externalDisorderScreenerGroup.WereABullyOrThreatendedOtherPeople
                         };
            GetScores(values, out pastMonth, out past90Days, out pastYear, out lifetime);
        }

        private void GetInternalDisorderScreenerScore(GainShortScreener gainShortScreener, out int pastMonth, out int past90Days, out int pastYear, out int lifetime)
        {
            var internalDisorderScreenerGroup = gainShortScreener.TotalDisorderScreenerGroup.InternalizingDisorderScreenerGroup;
            var values = new List<LastTimeFrequency>
                         {
                             internalDisorderScreenerGroup.BecomingVeryDistresstedAndUpsetAboutPast,
                             internalDisorderScreenerGroup.FeelingVeryAnxiousNervousTenseScaredPanicked,
                             internalDisorderScreenerGroup.FeelingVeryTrappedLonelySadBlueDepressedHopelessAboutFuture,
                             internalDisorderScreenerGroup.SeeingOrHearingThingsNoOneElseCouldSeeOrHear,
                             internalDisorderScreenerGroup.SleepTrouble,
                             internalDisorderScreenerGroup.ThinkingAboutEndingYourLife
                         };
            GetScores(values, out pastMonth, out past90Days, out pastYear, out lifetime);
        }

        private void GetScores ( IEnumerable<LastTimeFrequency> lastTimeFrequencies, out int pastMonth, out int past90Days, out int pastYear, out int lifetime )
        {
            pastMonth = past90Days = pastYear = lifetime = 0;
            foreach ( var lastTimeFrequency in lastTimeFrequencies )
            {
                if ( lastTimeFrequency == LastTimeFrequency.PastMonth )
                {
                    pastMonth++;
                    past90Days++;
                    pastYear++;
                    lifetime++;
                }
                else if ( lastTimeFrequency == LastTimeFrequency.TwoToThreeMonths )
                {
                    past90Days++;
                    pastYear++;
                    lifetime++;
                }
                else if ( lastTimeFrequency == LastTimeFrequency.FourToTwelveMonths )
                {
                    pastYear++;
                    lifetime++;
                }
                else if ( lastTimeFrequency == LastTimeFrequency.OnePlusYears )
                {
                    lifetime++;
                }
            }
        }

        private void GetSubstanceDisorderScreenerScore(GainShortScreener gainShortScreener, out int pastMonth, out int past90Days, out int pastYear, out int lifetime)
        {
            var substanceDisorderGroup = gainShortScreener.TotalDisorderScreenerGroup.SubstanceDisorderScreenerGroup;
            var values = new List<LastTimeFrequency>
                         {
                             substanceDisorderGroup.KeptUsingAlcoholOrDrugsCausingSocialProblems,
                             substanceDisorderGroup.SpentTimeGettingAlcoholOrOtherDrugs,
                             substanceDisorderGroup.UsedAlcoholOrOtherDrugsWeekly,
                             substanceDisorderGroup.YouHadWithdrawalProblemsFromAlcoholOrOtherDrugs,
                             substanceDisorderGroup.YourUseOfAlcoholOrDrugsCauseYouToGiveUp
                         };
            GetScores(values, out pastMonth, out past90Days, out pastYear, out lifetime);
        }

        #endregion
    }
}