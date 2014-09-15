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

namespace ProCenter.Domain.Gpra.Lookups
{
    #region Using Statements

    using System.Collections.Generic;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The interview information lookups class.</summary>
    public class InterviewInformationLookups
    {
        #region Static Fields

        public static readonly Lookup ClientInRecovery = new Lookup (
            codedConcept:
                new CodedConcept ( code: "M00000_1", codeSystem: CodeSystems.Obhita, name: "ClientInRecovery" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup Discharge = new Lookup (
            codedConcept:
                new CodedConcept ( code: "R00000_3", codeSystem: CodeSystems.Obhita, name: "Discharge" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup Intake = new Lookup (
            codedConcept: new CodedConcept ( code: "R00000_0", codeSystem: CodeSystems.Obhita, name: "Intake" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup SixMonthFollowUp = new Lookup (
            codedConcept:
                new CodedConcept ( code: "R00000_1", codeSystem: CodeSystems.Obhita, name: "SixMonthFollowUp" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup ThreeMonthFollowUp = new Lookup (
            codedConcept:
                new CodedConcept ( code: "R00000_2", codeSystem: CodeSystems.Obhita, name: "ClientInRecovery" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup TreatmentClient = new Lookup (
            codedConcept: new CodedConcept ( code: "M00000_0", codeSystem: CodeSystems.Obhita, name: "TreatmentClient" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly List<Lookup> PatientType = new List<Lookup>()
                                                          {
                                                              TreatmentClient,
                                                              ClientInRecovery,
                                                          };

        public static readonly List<Lookup> InterviewType = new List<Lookup>()
                                                            {
                                                                Intake,
                                                                SixMonthFollowUp,
                                                                ThreeMonthFollowUp,
                                                                Discharge
                                                            };

        #endregion
    }
}