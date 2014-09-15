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

    /// <summary>The treatment recovery lookups class.</summary>
    public class TreatmentRecoveryLookups
    {
        #region Static Fields

        public static readonly Lookup Considerably = new Lookup (
            codedConcept: new CodedConcept ( code: "P00000_3", codeSystem: CodeSystems.Obhita, name: "Considerably" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Excellent = new Lookup (
            codedConcept: new CodedConcept ( code: "N00000_0", codeSystem: CodeSystems.Obhita, name: "Excellent" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Extremely = new Lookup (
            codedConcept: new CodedConcept ( code: "P00000_4", codeSystem: CodeSystems.Obhita, name: "Extremely" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup Fair = new Lookup (
            codedConcept: new CodedConcept ( code: "N00000_3", codeSystem: CodeSystems.Obhita, name: "Fair" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Good = new Lookup (
            codedConcept: new CodedConcept ( code: "N00000_2", codeSystem: CodeSystems.Obhita, name: "Good" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup Moderately = new Lookup (
            codedConcept: new CodedConcept ( code: "P00000_2", codeSystem: CodeSystems.Obhita, name: "Moderately" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup NotAtAll = new Lookup (
            codedConcept: new CodedConcept ( code: "P00000_0", codeSystem: CodeSystems.Obhita, name: "NotAtAll" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Poor = new Lookup (
            codedConcept: new CodedConcept ( code: "N00000_4", codeSystem: CodeSystems.Obhita, name: "Poor" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup Slightly = new Lookup (
            codedConcept:
                new CodedConcept ( code: "P00000_1", codeSystem: CodeSystems.Obhita, name: "Slightly" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup VeryGood = new Lookup (
            codedConcept:
                new CodedConcept ( code: "N00000_1", codeSystem: CodeSystems.Obhita, name: "VeryGood" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly List<Lookup> HealthRating = new List<Lookup>()
                                                           {
                                                               Excellent,
                                                               VeryGood,
                                                               Good,
                                                               Fair,
                                                               Poor
                                                           };

        public static readonly List<Lookup> PsychologicalImpact = new List<Lookup>()
                                                                  {
                                                                      NotAtAll,
                                                                      Slightly,
                                                                      Moderately,
                                                                      Considerably,
                                                                      Extremely
                                                                  };

        #endregion
    }
}