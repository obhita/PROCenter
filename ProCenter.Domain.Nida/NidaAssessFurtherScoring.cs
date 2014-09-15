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

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>
    /// The NidaAssessFurtherScoring class.
    /// </summary>
    public class NidaAssessFurtherScoring
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NidaAssessFurtherScoring" /> class.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        public NidaAssessFurtherScoring ( AssessmentInstance assessment )
        {
            var nidaAssessFurther = new NidaAssessFurther ( assessment );
            var hasDailyUseSubstance =
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceMarijuanaPersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceCocainePersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOpioidPersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceStimulantPersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceSedativePersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOtherSubstanceofAbusePersonalMedicalHistoryFrequency == DrugUseFrequency.DailyOrAlmostDaily;

            var hasWeeklyUseSubstance =
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOpioidPersonalMedicalHistoryFrequency == DrugUseFrequency.Weekly ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceCocainePersonalMedicalHistoryFrequency == DrugUseFrequency.Weekly ||
                nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceStimulantPersonalMedicalHistoryFrequency == DrugUseFrequency.Weekly;

            var score =
                hasDailyUseSubstance ||
                hasWeeklyUseSubstance ||
                (
                    nidaAssessFurther.InjectionGroup.SubstanceAbuseIllicitSubstanceIntravenousRouteofAdministrationPersonalMedicalHistoryInd2 &&
                    nidaAssessFurther.InjectionGroup.SubstanceAbuseIllicitSubstanceIntravenousRouteofAdministrationPersonalMedicalHistoryFrequency
                    == InjectionFrequency.InThePast90Days
                    )
                ||
                nidaAssessFurther.SudTreatmentGroup.SubstanceAbuseSubstanceRelatedDisorderCurrentTherapyPersonalMedicalHistoryInd2;
            TotalScore = score;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public bool TotalScore { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return TotalScore.ToString();
        }

        #endregion
    }
}