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

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System.Linq;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The drug abuse screening test scoring engine class.</summary>
    public class DrugAbuseScreeningTestScoringEngine : IScoringEngine
    {
        #region Public Properties

        /// <summary>
        /// Gets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName
        {
            get { return typeof(DrugAbuseScreeningTest).Name; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Calculates the score.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        public void CalculateScore ( AssessmentInstance assessment )
        {
            var dast = new DrugAbuseScreeningTest ( assessment );
            var count = assessment.ItemInstances.Count ( i => ( bool.Parse ( i.Value.ToString () ) ) );
            if ( dast.SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductCessationAbilityPersonalMedicalHistoryInd2 )
            {
                count--;
            }

            var guidance = count <= 2
                ? new CodedConcept ( CodeSystems.Obhita, "guidance_0_to_2", "guidance_0_to_2" )
                : new CodedConcept ( CodeSystems.Obhita, "guidance_3_and_up", "guidance_3_and_up" );

            assessment.ScoreComplete ( new CodedConcept ( CodeSystems.Obhita, string.Empty, string.Empty ), count, false, guidance );
        }

        #endregion
    }
}