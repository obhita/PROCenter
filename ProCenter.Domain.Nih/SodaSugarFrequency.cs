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
    /// Lookup for fast food frequency.
    /// </summary>
    public class SodaSugarFrequency : Lookup
    {
        #region Static Fields

        /// <summary>
        /// The less than one time.
        /// </summary>
        public static readonly SodaSugarFrequency LessThanOneTime = new SodaSugarFrequency
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "C10001_0", codeSystem: CodeSystems.Obhita, name: "LessThanOneTime"),
                                                                         Value = 1,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The one to three times.
        /// </summary>
        public static readonly SodaSugarFrequency OneToTwoTimes = new SodaSugarFrequency
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "C10001_1", codeSystem: CodeSystems.Obhita, name: "OneToTwoTimes"),
                                                                        Value = 2,
                                                                        SortOrder = 2
                                                                    };

        /// <summary>
        /// The four or more times.
        /// </summary>
        public static readonly SodaSugarFrequency ThreeOrMoreTimes = new SodaSugarFrequency
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "C10001_2", codeSystem: CodeSystems.Obhita, name: "ThreeOrMoreTimes"),
                                                                        Value = 3,
                                                                        SortOrder = 3
                                                                    };
        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="SodaSugarFrequency" /> class.</summary>
        protected internal SodaSugarFrequency ()
        {
        }

        #endregion
    }
}