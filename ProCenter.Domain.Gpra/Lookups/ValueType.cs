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

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The value type class.</summary>
    public static class ValueType
    {
        #region Static Fields

        public static readonly Lookup Count = new Lookup ( new CodedConcept ( code: "C25463", codeSystem: CodeSystems.Nci, name: "Count" ) );

        public static readonly Lookup DischargeLookups = new Lookup ( new CodedConcept ( code: "T00000", codeSystem: CodeSystems.Obhita, name: "DischargeLookups" ) );

        public static readonly Lookup DrugAlcoholLookups = new Lookup ( new CodedConcept ( code: "B00000", codeSystem: CodeSystems.Obhita, name: "DrugAlcoholLookups" ) );

        public static readonly Lookup FamilyLivingSituationLookups =
            new Lookup ( new CodedConcept ( code: "F00000", codeSystem: CodeSystems.Obhita, name: "FamilyLivingSituationLookups" ) );

        public static readonly Lookup FollowUpLookups = new Lookup ( new CodedConcept ( code: "S00000", codeSystem: CodeSystems.Obhita, name: "FollowUpLookups" ) );

        public static readonly Lookup Frequency = new Lookup ( new CodedConcept ( code: "C25515", codeSystem: CodeSystems.Nci, name: "Frequency" ) );

        public static readonly Lookup Gender = new Lookup ( new CodedConcept ( code: "A00000", codeSystem: CodeSystems.Obhita, name: "Gender" ) );

        public static readonly Lookup InterviewInformationLookups =
            new Lookup ( new CodedConcept ( code: "M00000", codeSystem: CodeSystems.Obhita, name: "InterviewInformationLookups" ) );

        public static readonly Lookup PlannedServicesLookups = new Lookup ( new CodedConcept ( code: "C00000", codeSystem: CodeSystems.Obhita, name: "PlannedServicesLookups" ) );

        public static readonly Lookup ProfessionalInformationLookups =
            new Lookup ( new CodedConcept ( code: "J00000", codeSystem: CodeSystems.Obhita, name: "ProfessionalInformationLookups" ) );

        public static readonly Lookup Score = new Lookup ( new CodedConcept ( code: "C25338", codeSystem: CodeSystems.Nci, name: "Score" ) );

        public static readonly Lookup SocialConnectednessLookups =
            new Lookup ( new CodedConcept ( code: "Q00000", codeSystem: CodeSystems.Obhita, name: "SocialConnectednessLookups" ) );

        public static readonly Lookup Specify = new Lookup ( new CodedConcept ( code: "C25685", codeSystem: CodeSystems.Nci, name: "Specify" ) );

        public static readonly Lookup TreatmentRecoveryLookups = new Lookup (
            new CodedConcept ( code: "N00000", codeSystem: CodeSystems.Obhita, name: "TreatmentRecoveryLookups" ) );

        public static readonly Lookup YesOrNoResponse = new Lookup ( new CodedConcept ( code: "C38147", codeSystem: CodeSystems.Nci, name: "Yes or No Response" ) );

        #endregion
    }
}