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

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The value type class.</summary>
    public class NidaValueType : IValueTypeProvider
    {
        #region Constants

        public const string CountCode = "C25463";

        public const string FrequencyCode = "C25515";

        public const string ScoreCode = "C25338";

        public const string SpecifyCode = "C25685";

        public const string YesOrNoResponseCode = "C38147";

        #endregion

        #region Static Fields

        public static readonly Lookup Count = new Lookup ( new CodedConcept ( code: CountCode, codeSystem: CodeSystems.Nci, name: "Count" ) );

        public static readonly Lookup Frequency = new Lookup ( new CodedConcept ( code: FrequencyCode, codeSystem: CodeSystems.Nci, name: "Frequency" ) );

        public static readonly Lookup Score = new Lookup ( new CodedConcept ( code: ScoreCode, codeSystem: CodeSystems.Nci, name: "Score" ) );

        public static readonly Lookup Specify = new Lookup ( new CodedConcept ( code: SpecifyCode, codeSystem: CodeSystems.Nci, name: "Specify" ) );

        public static readonly Lookup YesOrNoResponse = new Lookup ( new CodedConcept ( code: YesOrNoResponseCode, codeSystem: CodeSystems.Nci, name: "Yes or No Response" ) );

        private static readonly List<Lookup> _lookups = new List<Lookup> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes static members of the <see cref="NidaValueType" /> class.</summary>
        static NidaValueType ()
        {
            foreach (var propertyInfo in typeof(NidaValueType).GetProperties())
            {
                _lookups.Add ( propertyInfo.GetValue ( null ) as Lookup );
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the by code.</summary>
        /// <param name="code">The code.</param>
        /// <returns>A <see cref="Lookup" />.</returns>
        public Lookup GetByCode ( string code )
        {
            return _lookups.FirstOrDefault ( c => c.CodedConcept.Code == code );
        }

        #endregion
    }
}