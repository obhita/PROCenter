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

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The frequency class.</summary>
    public class InjectionFrequency : Lookup
    {
        #region Static Fields

        /// <summary>The in the past90 days.</summary>
        public static readonly InjectionFrequency InThePast90Days = new InjectionFrequency
                                                                    {
                                                                        CodedConcept = new CodedConcept ( code: "C25515_4", codeSystem: CodeSystems.Nci, name: "Inthepast90days" ),
                                                                        Value = 4,
                                                                        SortOrder = 4
                                                                    };

        /// <summary>The in the past year.</summary>
        public static readonly InjectionFrequency InThePastYear = new InjectionFrequency
                                                                  {
                                                                      CodedConcept = new CodedConcept ( code: "C25515_5", codeSystem: CodeSystems.Nci, name: "Inthepastyear" ),
                                                                      Value = 5,
                                                                      SortOrder = 5
                                                                  };

        /// <summary>The over a year ago.</summary>
        public static readonly InjectionFrequency OverAYearAgo = new InjectionFrequency
                                                                 {
                                                                     CodedConcept = new CodedConcept ( code: "C25515_6", codeSystem: CodeSystems.Nci, name: "Overayearago" ),
                                                                     Value = 6,
                                                                     SortOrder = 6
                                                                 };

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="InjectionFrequency" /> class.</summary>
        protected internal InjectionFrequency ()
        {
        }

        #endregion
    }
}