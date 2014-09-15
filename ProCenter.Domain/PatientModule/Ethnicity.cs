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

namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>Class for defining ethnicity lookups.</summary>
    public class Ethnicity : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem _ethnicityCodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     AfricanAmerican = 0.
        /// </summary>
        public static readonly Ethnicity AfricanAmerican = new Ethnicity
                                                        {
                                                            CodedConcept = new CodedConcept ( code: "AfricanAmerican", codeSystem: _ethnicityCodeSystem, name: "AfricanAmerican" ),
                                                            SortOrder = 3,
                                                            Value = 2
                                                        };

        /// <summary>
        ///     AlaskanNative = 0.
        /// </summary>
        public static readonly Ethnicity AlaskanNative = new Ethnicity
                                                         {
                                                             CodedConcept = new CodedConcept ( code: "AlaskanNative", codeSystem: _ethnicityCodeSystem, name: "AlaskanNative" ),
                                                             SortOrder = 6,
                                                             Value = 5
                                                         };

        /// <summary>
        ///     Asian = 0.
        /// </summary>
        public static readonly Ethnicity Asian = new Ethnicity
                                                 {
                                                     CodedConcept = new CodedConcept ( code: "Asian", codeSystem: _ethnicityCodeSystem, name: "Asian" ),
                                                     SortOrder = 8,
                                                     Value = 7
                                                 };

        /// <summary>
        ///     Caucasian = 0.
        /// </summary>
        public static readonly Ethnicity Caucasian = new Ethnicity
                                                     {
                                                         CodedConcept = new CodedConcept ( code: "Caucasian", codeSystem: _ethnicityCodeSystem, name: "Caucasian" ),
                                                         SortOrder = 2,
                                                         Value = 1
                                                     };

        /// <summary>
        ///     Hispanic = 0.
        /// </summary>
        public static readonly Ethnicity Hispanic = new Ethnicity
                                                    {
                                                        CodedConcept = new CodedConcept ( code: "Hispanic", codeSystem: _ethnicityCodeSystem, name: "Hispanic" ),
                                                        SortOrder = 7,
                                                        Value = 6
                                                    };

        /// <summary>
        ///     NativeAmerican = 0.
        /// </summary>
        public static readonly Ethnicity NativeAmerican = new Ethnicity
                                                          {
                                                              CodedConcept = new CodedConcept ( code: "NativeAmerican", codeSystem: _ethnicityCodeSystem, name: "NativeAmerican" ),
                                                              SortOrder = 4,
                                                              Value = 3
                                                          };

        /// <summary>
        ///     PacificIslander = 0.
        /// </summary>
        public static readonly Ethnicity PacificIslander = new Ethnicity
                                                        {
                                                            CodedConcept = new CodedConcept ( code: "PacificIslander", codeSystem: _ethnicityCodeSystem, name: "PacificIslander" ),
                                                            SortOrder = 5,
                                                            Value = 4
                                                        };

        /// <summary>
        ///     Undeclared = 0.
        /// </summary>
        public static readonly Ethnicity Undeclared = new Ethnicity
                                                      {
                                                          CodedConcept = new CodedConcept ( code: "Undeclared", codeSystem: _ethnicityCodeSystem, name: "Undeclared" ),
                                                          SortOrder = 1,
                                                          Value = 0
                                                      };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ethnicity"/> class.
        /// </summary>
        protected internal Ethnicity ()
        {
        }

        #endregion
    }
}