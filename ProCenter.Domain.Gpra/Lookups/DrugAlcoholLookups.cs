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
//  * DIRECT, INDIRECT, IObhitaDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

#endregion

namespace ProCenter.Domain.Gpra.Lookups
{
    #region Using Statements

    using System.Collections.Generic;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The drug alcohol lookups class.</summary>
    public class DrugAlcoholLookups
    {
        #region Static Fields

        public static readonly Lookup Always = new Lookup (
            codedConcept: new CodedConcept ( code: "D00000_0", codeSystem: CodeSystems.Obhita, name: "Always" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Half = new Lookup (
            codedConcept: new CodedConcept ( code: "D00000_2", codeSystem: CodeSystems.Obhita, name: "Half" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup Iv = new Lookup (
            codedConcept: new CodedConcept ( code: "B00000_4", codeSystem: CodeSystems.Obhita, name: "IV" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup LessThanHalf = new Lookup (
            codedConcept: new CodedConcept ( code: "D00000_3", codeSystem: CodeSystems.Obhita, name: "LessThanHalf" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup MoreThanHalf = new Lookup (
            codedConcept:
                new CodedConcept ( code: "D00000_1", codeSystem: CodeSystems.Obhita, name: "MoreThanHalf" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup Nasal = new Lookup (
            codedConcept:
                new CodedConcept ( code: "B00000_1", codeSystem: CodeSystems.Obhita, name: "Nasal" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup Never = new Lookup (
            codedConcept: new CodedConcept ( code: "D00000_4", codeSystem: CodeSystems.Obhita, name: "Never" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup NonIvInjection = new Lookup (
            codedConcept: new CodedConcept ( code: "B00000_3", codeSystem: CodeSystems.Obhita, name: "NonIVInjection" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Oral = new Lookup (
            codedConcept: new CodedConcept ( code: "B00000_0", codeSystem: CodeSystems.Obhita, name: "Oral" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Smoking = new Lookup (
            codedConcept: new CodedConcept ( code: "B00000_2", codeSystem: CodeSystems.Obhita, name: "Smoking" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly List<Lookup> Frequency = new List<Lookup>()
                                                        {
                                                            Always,
                                                            MoreThanHalf,
                                                            Half,
                                                            LessThanHalf,
                                                            Never
                                                        };

        /// <summary>Gets the administration route.</summary>
        /// <value>The administration route.</value>
        public static List<Lookup> AdministrationRoute
        {
            get
            {
                return new List<Lookup> ()
                       {
                           Oral,
                           Nasal,
                           Smoking,
                           NonIvInjection,
                           Iv
                       };
            }
        }

        #endregion
    }
}