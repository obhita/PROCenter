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

namespace ProCenter.Domain.Psc
{
    #region Using Statements

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>
    /// Lookup for time frequency.
    /// </summary>
    public class TimeFrequency : Lookup
    {
        #region Static Fields

        /// <summary>
        /// The never frequency.
        /// </summary>
        public static readonly TimeFrequency Never = new TimeFrequency
                                                                     {
                                                                         CodedConcept =
                                                                             new CodedConcept(code: "A50001_0", codeSystem: CodeSystems.Obhita, name: "Never"),
                                                                         Value = 0,
                                                                         SortOrder = 1
                                                                     };

        /// <summary>
        /// The sometimes frequency.
        /// </summary>
        public static readonly TimeFrequency Sometimes = new TimeFrequency
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "A50001_1", codeSystem: CodeSystems.Obhita, name: "Sometimes"),
                                                                        Value = 1,
                                                                        SortOrder = 2
                                                                    };

        /// <summary>
        /// The often frequency.
        /// </summary>
        public static readonly TimeFrequency Often = new TimeFrequency
                                                                    {
                                                                        CodedConcept =
                                                                            new CodedConcept(code: "A50001_2", codeSystem: CodeSystems.Obhita, name: "Often"),
                                                                        Value = 2,
                                                                        SortOrder = 3
                                                                    };

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="TimeFrequency" /> class.</summary>
        protected internal TimeFrequency ()
        {
        }

        #endregion
    }
}