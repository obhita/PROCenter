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

    /// <summary>Class for defining religion lookups.</summary>
    public class Religion : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem _religionCodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     Catholic = 2.
        /// </summary>
        public static readonly Religion Catholic = new Religion
                                                   {
                                                       CodedConcept = new CodedConcept ( code: "Catholic", codeSystem: _religionCodeSystem, name: "Catholic" ),
                                                       SortOrder = 2,
                                                       Value = 2
                                                   };

        /// <summary>
        ///     Islamic = 4.
        /// </summary>
        public static readonly Religion Islamic = new Religion
                                                  {
                                                      CodedConcept = new CodedConcept ( code: "Islamic", codeSystem: _religionCodeSystem, name: "Islamic" ),
                                                      SortOrder = 4,
                                                      Value = 4
                                                  };

        /// <summary>
        ///     Jewish = 3.
        /// </summary>
        public static readonly Religion Jewish = new Religion
                                                 {
                                                     CodedConcept = new CodedConcept ( code: "Jewish", codeSystem: _religionCodeSystem, name: "Jewish" ),
                                                     SortOrder = 3,
                                                     Value = 3
                                                 };

        /// <summary>
        ///     None = 6.
        /// </summary>
        public static readonly Religion None = new Religion
                                               {
                                                   CodedConcept = new CodedConcept ( code: "None", codeSystem: _religionCodeSystem, name: "None" ),
                                                   SortOrder = 6,
                                                   Value = 6
                                               };

        /// <summary>
        ///     Other = 5.
        /// </summary>
        public static readonly Religion Other = new Religion
                                                {
                                                    CodedConcept = new CodedConcept ( code: "Other", codeSystem: _religionCodeSystem, name: "Other" ),
                                                    SortOrder = 5,
                                                    Value = 5
                                                };

        /// <summary>
        ///     Protestant = 1.
        /// </summary>
        public static readonly Religion Protestant = new Religion
                                                     {
                                                         CodedConcept = new CodedConcept ( code: "Protestant", codeSystem: _religionCodeSystem, name: "Protestant" ),
                                                         SortOrder = 1,
                                                         Value = 1
                                                     };


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Religion"/> class.
        /// </summary>
        protected internal Religion ()
        {
        }

        #endregion
    }
}