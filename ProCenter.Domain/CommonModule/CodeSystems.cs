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

namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>Static class containing common code systems.</summary>
    public static class CodeSystems
    {
        #region Constants

        public const string LoincCode = "2.16.840.1.113883.6.1";

        public const string NciCode = "2.16.840.1.113883.3.26";

        public const string ObhitaCode = "";

        public const string SnomedCtCode = "2.16.840.1.113883.6.96";

        #endregion

        #region Static Fields

        private static readonly List<CodeSystem> _codeSystems = new List<CodeSystem> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes static members of the <see cref="CodeSystems" /> class.</summary>
        static CodeSystems ()
        {
            foreach ( var propertyInfo in typeof(CodeSystems).GetProperties ( ) )
            {
                _codeSystems.Add ( propertyInfo.GetValue ( null ) as CodeSystem );
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the loinc code system.
        /// </summary>
        /// <value>
        ///     The loinc.
        /// </value>
        public static CodeSystem Loinc
        {
            get { return new CodeSystem ( code: LoincCode, version: string.Empty, name: "LOINC" ); }
        }

        /// <summary>
        ///     Gets the national cancer institute code system.
        /// </summary>
        /// <value>
        ///     The nci.
        /// </value>
        public static CodeSystem Nci
        {
            get { return new CodeSystem ( code: NciCode, version: string.Empty, name: "NationalCancerInstitute" ); }
        }

        /// <summary>
        ///     Gets the obhita code system.
        /// </summary>
        /// <value>
        ///     The obhita.
        /// </value>
        public static CodeSystem Obhita
        {
            get { return new CodeSystem ( code: ObhitaCode, version: string.Empty, name: "OBHITA" ); }
        }

        /// <summary>
        ///     Gets the snomed CT code system.
        /// </summary>
        /// <value>
        ///     The snomed ct.
        /// </value>
        public static CodeSystem SnomedCt
        {
            get { return new CodeSystem ( code: SnomedCtCode, version: string.Empty, name: "SNOMEDCT" ); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the by code.</summary>
        /// <param name="code">The code.</param>
        /// <returns>A <see cref="CodeSystem" />.</returns>
        public static CodeSystem GetByCode ( string code )
        {
            return _codeSystems.FirstOrDefault ( c => c.Code == code );
        }

        #endregion
    }
}