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

    /// <summary>The discharge lookups class.</summary>
    public class DischargeLookups
    {
        #region Static Fields

        public static readonly Lookup Completion = new Lookup (
            codedConcept: new CodedConcept ( code: "T00000_0", codeSystem: CodeSystems.Obhita, name: "Completion" ),
            value: 0,
            sortOrder: 0 );

        public static readonly Lookup Death = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_11", codeSystem: CodeSystems.Obhita, name: "Death" ),
            value: 11,
            sortOrder: 11 );

        public static readonly Lookup IncarceratedOffenseWhileInTreatmentNonSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_7", codeSystem: CodeSystems.Obhita, name: "IncarceratedOffenseWhileInTreatmentNonSatisfactoryProgress" ),
            value: 7,
            sortOrder: 7
            );

        public static readonly Lookup IncarceratedOffenseWhileInTreatmentSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_6", codeSystem: CodeSystems.Obhita, name: "IncarceratedOffenseWhileInTreatmentSatisfactoryProgress" ),
            value: 6,
            sortOrder: 6
            );

        public static readonly Lookup IncarceratedOldWarrentNonSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_9", codeSystem: CodeSystems.Obhita, name: "IncarceratedOldWarrentNonSatisfactoryProgress" ),
            value: 9,
            sortOrder: 9
            );

        public static readonly Lookup IncarceratedOldWarrentSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_8", codeSystem: CodeSystems.Obhita, name: "IncarceratedOldWarrentSatisfactoryProgress" ),
            value: 8,
            sortOrder: 8
            );

        public static readonly Lookup InvoluntarilyDschargedNonParticipation = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_2", codeSystem: CodeSystems.Obhita, name: "InvoluntarilyDschargedNonParticipation" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup InvoluntarilyDschargedRulesViolation = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_3", codeSystem: CodeSystems.Obhita, name: "InvoluntarilyDschargedRulesViolation" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup LeftOnOwnWithOutSatisfactoryProgress = new Lookup (
            codedConcept:
                new CodedConcept ( code: "U00000_1", codeSystem: CodeSystems.Obhita, name: "LeftOnOwnWithOutSatisfactoryProgress" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup LeftOnOwnWithSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_0", codeSystem: CodeSystems.Obhita, name: "LeftOnOwnWithSatisfactoryProgress" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Other = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_12", codeSystem: CodeSystems.Obhita, name: "Other" ),
            value: 12,
            sortOrder: 12
            );

        public static readonly Lookup ReferredToAnotherProgramNonSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_5", codeSystem: CodeSystems.Obhita, name: "ReferredToAnotherProgramNonSatisfactoryProgress" ),
            value: 5,
            sortOrder: 5
            );

        public static readonly Lookup ReferredToAnotherProgramSatisfactoryProgress = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_4", codeSystem: CodeSystems.Obhita, name: "ReferredToAnotherProgramSatisfactoryProgress" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup Termination = new Lookup (
            codedConcept:
                new CodedConcept ( code: "T00000_1", codeSystem: CodeSystems.Obhita, name: "Termination" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup TransferredHealthReasons = new Lookup (
            codedConcept: new CodedConcept ( code: "U00000_10", codeSystem: CodeSystems.Obhita, name: "TransferredHealthReasons" ),
            value: 10,
            sortOrder: 10
            );

        public static readonly List<Lookup> DischargeStatus = new List<Lookup>()
                                                              {
                                                                  Completion,
                                                                  Termination,
                                                              };

        public static readonly List<Lookup> DischargeTerminationReason = new List<Lookup>()
                                                                         {
                                                                             LeftOnOwnWithSatisfactoryProgress,
                                                                             LeftOnOwnWithOutSatisfactoryProgress,
                                                                             InvoluntarilyDschargedNonParticipation,
                                                                             InvoluntarilyDschargedRulesViolation,
                                                                             ReferredToAnotherProgramSatisfactoryProgress,
                                                                             ReferredToAnotherProgramNonSatisfactoryProgress,
                                                                             IncarceratedOffenseWhileInTreatmentSatisfactoryProgress,
                                                                             IncarceratedOffenseWhileInTreatmentNonSatisfactoryProgress,
                                                                             IncarceratedOldWarrentSatisfactoryProgress,
                                                                             IncarceratedOldWarrentNonSatisfactoryProgress,
                                                                             TransferredHealthReasons,
                                                                             Death,
                                                                             Other
                                                                         };

        #endregion
    }
}