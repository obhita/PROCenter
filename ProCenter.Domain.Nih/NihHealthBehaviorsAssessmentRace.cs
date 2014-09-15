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
    /// Class for race lookup.
    /// </summary>
    public class NihHealthBehaviorsAssessmentRace : Lookup
    {
        #region Static Fields

        /// <summary>
        /// The white.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace White = new NihHealthBehaviorsAssessmentRace
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "K10001_0", codeSystem: CodeSystems.Obhita, name: "White"),
                                                                         Value = 1,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The black.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Black = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_1", codeSystem: CodeSystems.Obhita, name: "Black"),
                                                                        Value = 2,
                                                                        SortOrder = 2
                                                                    };
        
        /// <summary>
        /// The american indian.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace AmericanIndian = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_2", codeSystem: CodeSystems.Obhita, name: "AmericanIndian"),
                                                                        Value = 3,
                                                                        SortOrder = 3
                                                                    };

        /// <summary>
        /// The asian indian.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace AsianIndian = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_3", codeSystem: CodeSystems.Obhita, name: "AsianIndian"),
                                                                        Value = 4,
                                                                        SortOrder = 4
                                                                    };

        /// <summary>
        /// The chinese.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Chinese = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_4", codeSystem: CodeSystems.Obhita, name: "Chinese"),
                                                                        Value = 5,
                                                                        SortOrder = 5
                                                                    };
        
        /// <summary>
        /// The filipino.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Filipino = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_5", codeSystem: CodeSystems.Obhita, name: "Filipino"),
                                                                        Value = 6,
                                                                        SortOrder = 6
                                                                    };
        
        /// <summary>
        /// The japanese.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Japanese = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_6", codeSystem: CodeSystems.Obhita, name: "Japanese"),
                                                                        Value = 7,
                                                                        SortOrder = 7
                                                                    };
        
        /// <summary>
        /// The korean.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Korean = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_7", codeSystem: CodeSystems.Obhita, name: "Korean"),
                                                                        Value = 8,
                                                                        SortOrder = 8
                                                                    };
        
        /// <summary>
        /// The vietnamese.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Vietnamese = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_8", codeSystem: CodeSystems.Obhita, name: "Vietnamese"),
                                                                        Value = 9,
                                                                        SortOrder = 9
                                                                    };
        
        /// <summary>
        /// The other asian.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace OtherAsian = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_9", codeSystem: CodeSystems.Obhita, name: "OtherAsian"),
                                                                        Value = 10,
                                                                        SortOrder = 10
                                                                    };

        /// <summary>
        /// The native hawaiian.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace NativeHawaiian = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_10", codeSystem: CodeSystems.Obhita, name: "NativeHawaiian"),
                                                                        Value = 11,
                                                                        SortOrder = 11
                                                                    };

        /// <summary>
        /// The guamanian.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Guamanian = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_11", codeSystem: CodeSystems.Obhita, name: "Guamanian"),
                                                                        Value = 12,
                                                                        SortOrder = 12
                                                                    };

        /// <summary>
        /// The somoan.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace Somoan = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_12", codeSystem: CodeSystems.Obhita, name: "Somoan"),
                                                                        Value = 13,
                                                                        SortOrder = 13
                                                                    };

        /// <summary>
        /// The other pacific islander.
        /// </summary>
        public static readonly NihHealthBehaviorsAssessmentRace OtherPacificIslander = new NihHealthBehaviorsAssessmentRace
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "K10001_13", codeSystem: CodeSystems.Obhita, name: "OtherPacificIslander"),
                                                                        Value = 14,
                                                                        SortOrder = 14
                                                                    };
        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentRace" /> class.</summary>
        protected internal NihHealthBehaviorsAssessmentRace ()
        {
        }

        #endregion
    }
}