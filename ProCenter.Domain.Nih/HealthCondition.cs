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
    /// Class for Health Condition Lookup.
    /// </summary>
    public class HealthCondition : Lookup
    {
        #region Static Fields

        /// <summary>
        /// The excellent.
        /// </summary>
        public static readonly HealthCondition Excellent = new HealthCondition
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "F10001_0", codeSystem: CodeSystems.Obhita, name: "Excellent"),
                                                                         Value = 1,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The very good.
        /// </summary>
        public static readonly HealthCondition VeryGood = new HealthCondition
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "F10001_1", codeSystem: CodeSystems.Obhita, name: "VeryGood"),
                                                                        Value = 2,
                                                                        SortOrder = 2
                                                                    };

        /// <summary>
        /// The good.
        /// </summary>
        public static readonly HealthCondition Good = new HealthCondition
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "F10001_2", codeSystem: CodeSystems.Obhita, name: "Good"),
                                                                        Value = 3,
                                                                        SortOrder = 3
                                                                    };

        /// <summary>
        /// The fair.
        /// </summary>
        public static readonly HealthCondition Fair = new HealthCondition
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "F10001_3", codeSystem: CodeSystems.Obhita, name: "Fair"),
                                                                        Value = 4,
                                                                        SortOrder = 4
                                                                    };
        
        /// <summary>
        /// The poor.
        /// </summary>
        public static readonly HealthCondition Poor = new HealthCondition
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "F10001_4", codeSystem: CodeSystems.Obhita, name: "Poor"),
                                                                        Value = 5,
                                                                        SortOrder = 5
                                                                    };
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCondition"/> class.
        /// </summary>
        protected internal HealthCondition()
        {
        }

        #endregion
    }
}