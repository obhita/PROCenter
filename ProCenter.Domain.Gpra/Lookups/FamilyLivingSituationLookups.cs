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

    /// <summary>The family living sitation lookups class.</summary>
    public class FamilyLivingSitationLookups
    {
        #region Static Fields

        public static readonly Lookup Considerably = new Lookup (
            codedConcept: new CodedConcept ( code: "H00000_2", codeSystem: CodeSystems.Obhita, name: "Considerably" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup DormCollegeResidence = new Lookup (
            codedConcept: new CodedConcept ( code: "G00000_2", codeSystem: CodeSystems.Obhita, name: "DormCollegeResidence" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup Extremely = new Lookup (
            codedConcept: new CodedConcept ( code: "H00000_3", codeSystem: CodeSystems.Obhita, name: "Extremely" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup HalfwayHouse = new Lookup (
            codedConcept: new CodedConcept ( code: "G00000_3", codeSystem: CodeSystems.Obhita, name: "HalfwayHouse" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Housed = new Lookup (
            codedConcept: new CodedConcept ( code: "F00000_3", codeSystem: CodeSystems.Obhita, name: "Housed" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Institution = new Lookup (
            codedConcept: new CodedConcept ( code: "F00000_2", codeSystem: CodeSystems.Obhita, name: "Institution" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup NotApplicable = new Lookup (
            codedConcept: new CodedConcept ( code: "H00000_4", codeSystem: CodeSystems.Obhita, name: "NotApplicable" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup NotAtAll = new Lookup (
            codedConcept: new CodedConcept ( code: "H00000_0", codeSystem: CodeSystems.Obhita, name: "NotAtAll" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Other = new Lookup (
            codedConcept: new CodedConcept ( code: "G00000_5", codeSystem: CodeSystems.Obhita, name: "Other" ),
            value: 5,
            sortOrder: 5
            );

        public static readonly Lookup OwnRent = new Lookup (
            codedConcept: new CodedConcept ( code: "G00000_0", codeSystem: CodeSystems.Obhita, name: "OwnRent" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup ResidentialTreatment = new Lookup (
            codedConcept: new CodedConcept ( code: "G00000_4", codeSystem: CodeSystems.Obhita, name: "ResidentialTreatment" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup Shelter = new Lookup (
            codedConcept: new CodedConcept ( code: "F00000_0", codeSystem: CodeSystems.Obhita, name: "Shelter" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup SomeoneElsesHome = new Lookup (
            codedConcept:
                new CodedConcept ( code: "G00000_1", codeSystem: CodeSystems.Obhita, name: "SomeoneElsesHome" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup Somewhat = new Lookup (
            codedConcept:
                new CodedConcept ( code: "H00000_1", codeSystem: CodeSystems.Obhita, name: "Somewhat" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup StreetOutdoors = new Lookup (
            codedConcept:
                new CodedConcept ( code: "F00000_1", codeSystem: CodeSystems.Obhita, name: "StreetOutdoors" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly List<Lookup> Frequency = new List<Lookup>
                                                        {
                                                            NotAtAll,
                                                            Somewhat,
                                                            Considerably,
                                                            Extremely,
                                                            NotApplicable
                                                        };

        public static readonly List<Lookup> HousingType = new List<Lookup>()
                                                          {
                                                              OwnRent,
                                                              SomeoneElsesHome,
                                                              DormCollegeResidence,
                                                              HalfwayHouse,
                                                              ResidentialTreatment,
                                                              Other
                                                          };

        public static readonly List<Lookup> LivingSituation = new List<Lookup>()
                                                              {
                                                                  Shelter,
                                                                  StreetOutdoors,
                                                                  Institution,
                                                                  Housed
                                                              };

        #endregion
    }
}