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
    /// Class for Employment Status Lookup.
    /// </summary>
    public class NihHealthBehaviorsAssessmentEmploymentStatus : Lookup
    {
        #region Static Fields


        /// <summary>
        /// The employeed full time.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus EmployeedFullTime = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "M10001_0", codeSystem: CodeSystems.Obhita, name: "EmployeedFullTime"),
                                                                         Value = 1,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The employed part time.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus EmployedPartTime = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_1", codeSystem: CodeSystems.Obhita, name: "EmployedPartTime"),
                                                                        Value = 2,
                                                                        SortOrder = 2
                                                                    };

        /// <summary>
        /// The unemployed.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Unemployed = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_2", codeSystem: CodeSystems.Obhita, name: "Unemployed"),
                                                                        Value = 3,
                                                                        SortOrder = 3
                                                                    };

        /// <summary>
        /// The homemaker.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Homemaker = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_3", codeSystem: CodeSystems.Obhita, name: "Homemaker"),
                                                                        Value = 4,
                                                                        SortOrder = 4
                                                                    };

        /// <summary>
        /// The student.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Student = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_4", codeSystem: CodeSystems.Obhita, name: "Student"),
                                                                        Value = 5,
                                                                        SortOrder = 5
                                                                    };

        /// <summary>
        /// The disabled.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Disabled = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_5", codeSystem: CodeSystems.Obhita, name: "Disabled"),
                                                                        Value = 6,
                                                                        SortOrder = 6
                                                                    };

        /// <summary>
        /// The retired.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Retired = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_6", codeSystem: CodeSystems.Obhita, name: "Retired"),
                                                                        Value = 7,
                                                                        SortOrder = 7
                                                                    };

        /// <summary>
        /// The other.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentEmploymentStatus Other = new NihHealthBehaviorsAssessmentEmploymentStatus
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "M10001_7", codeSystem: CodeSystems.Obhita, name: "Other"),
                                                                        Value = 8,
                                                                        SortOrder = 8
                                                                    };

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentEmploymentStatus" /> class.</summary>
        protected internal NihHealthBehaviorsAssessmentEmploymentStatus ()
        {
        }

        #endregion
    }
}