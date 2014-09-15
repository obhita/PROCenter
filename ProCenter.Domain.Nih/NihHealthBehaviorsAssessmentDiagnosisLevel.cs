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

namespace ProCenter.Domain.Nih
{
    #region Using Statements

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>
    /// Lookup for time NihDiagnosisLevel.
    /// </summary>
    public class NihHealthBehaviorsAssessmentDiagnosisLevel : Lookup
    {
        #region Static Fields

        /// <summary>
        /// The Low.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentDiagnosisLevel Low = new NihHealthBehaviorsAssessmentDiagnosisLevel
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "A80001_0", codeSystem: CodeSystems.Obhita, name: "Low"),
                                                                         Value = 0,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The Medium.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentDiagnosisLevel Medium = new NihHealthBehaviorsAssessmentDiagnosisLevel
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "A80002_0", codeSystem: CodeSystems.Obhita, name: "Medium"),
                                                                         Value = 0,
                                                                         SortOrder = 2
                                                                     };

        /// <summary>
        /// The High.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentDiagnosisLevel High = new NihHealthBehaviorsAssessmentDiagnosisLevel
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "A80003_0", codeSystem: CodeSystems.Obhita, name: "High"),
                                                                         Value = 0,
                                                                         SortOrder = 2
                                                                     };
#endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentDiagnosisLevel" /> class.</summary>
        protected internal NihHealthBehaviorsAssessmentDiagnosisLevel()
        {
        }

        #endregion
    }
}