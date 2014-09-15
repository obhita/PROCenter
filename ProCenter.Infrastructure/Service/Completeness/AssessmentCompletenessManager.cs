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

namespace ProCenter.Infrastructure.Service.Completeness
{
    #region Using Statements

    using System.Linq;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The assessment completeness manager class.</summary>
    public class AssessmentCompletenessManager : IAssessmentCompletenessManager
    {
        #region Public Methods and Operators

        /// <summary>Calculates the completeness.</summary>
        /// <param name="assessment">The assessment.</param>
        /// <param name="sectionDefinition">The section definition.</param>
        /// <returns>A <see cref="CompletenessResults" />.</returns>
        public CompletenessResults CalculateCompleteness ( AssessmentInstance assessment, ItemDefinition sectionDefinition = null )
        {
            if ( sectionDefinition != null )
            {
                var requiredQuestions = AssessmentDefinition.GetAllItemDefinitionsOfTypeInContainer ( sectionDefinition, ItemType.Question ).Where ( i => i.GetIsRequired () );
                var totalRequired = requiredQuestions.Count ( );
                var skippedTotal = requiredQuestions.Select(i => i.CodedConcept.Code).Intersect(assessment.SkippedItemDefinitions.Select(i => i.CodedConcept.Code)).Count();
                var answeredTotal = requiredQuestions.Select ( i => i.CodedConcept.Code ).Intersect ( assessment.ItemInstances.Select ( i => i.ItemDefinitionCode ) ).Count ();
                return new CompletenessResults ( "Report", totalRequired - skippedTotal, answeredTotal );
            }
            return assessment.CalculateCompleteness ();
        }

        #endregion
    }
}