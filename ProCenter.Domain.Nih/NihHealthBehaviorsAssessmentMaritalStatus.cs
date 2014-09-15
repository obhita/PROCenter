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
    /// Lookup class for Marital Status.
    /// </summary>
    public class NihHealthBehaviorsAssessmentMaritalStatus : Lookup
    {
        #region Static Fields


        /// <summary>
        /// The married.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus Married = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "N10001_0", codeSystem: CodeSystems.Obhita, name: "Married"),
                                                                         Value = 1,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The divorced.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus Divorced = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "N10001_1", codeSystem: CodeSystems.Obhita, name: "Divorced"),
                                                                        Value = 2,
                                                                        SortOrder = 2
                                                                    };

        /// <summary>
        /// The separated.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus Separated = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "N10001_2", codeSystem: CodeSystems.Obhita, name: "Separated"),
                                                                        Value = 3,
                                                                        SortOrder = 3
                                                                    };

        /// <summary>
        /// The living as married.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus LivingAsMarried = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "N10001_3", codeSystem: CodeSystems.Obhita, name: "LivingAsMarried"),
                                                                        Value = 4,
                                                                        SortOrder = 4
                                                                    };

        /// <summary>
        /// The widowed.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus Widowed = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "N10001_4", codeSystem: CodeSystems.Obhita, name: "Widowed"),
                                                                        Value = 5,
                                                                        SortOrder = 5
                                                                    };

        /// <summary>
        /// The single.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentMaritalStatus Single = new NihHealthBehaviorsAssessmentMaritalStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "N10001_5", codeSystem: CodeSystems.Obhita, name: "Single"),
                                                                        Value = 6,
                                                                        SortOrder = 6
                                                                    };
        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentMaritalStatus" /> class.</summary>
        protected internal NihHealthBehaviorsAssessmentMaritalStatus ()
        {
        }

        #endregion
    }
}