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

    /// <summary>The follow up lookups class.</summary>
    public class FollowUpLookups
    {
        #region Static Fields

        public static readonly Lookup CompletedInSpecifiedWindow = new Lookup (
            codedConcept:
                new CodedConcept ( code: "S00000_1", codeSystem: CodeSystems.Obhita, name: "CompletedInSpecifiedWindow" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup CompletedOutSpecifiedWindow = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_2", codeSystem: CodeSystems.Obhita, name: "CompletedOutSpecifiedWindow" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup Deceased = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_0", codeSystem: CodeSystems.Obhita, name: "Deceased" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup LocatedButRefused = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_3", codeSystem: CodeSystems.Obhita, name: "LocatedButRefused" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup LocatedUnableGainAccess = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_5", codeSystem: CodeSystems.Obhita, name: "LocatedUnableGainAccess" ),
            value: 5,
            sortOrder: 5
            );

        public static readonly Lookup LocatedUnableGainInstitutionalAccess = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_4", codeSystem: CodeSystems.Obhita, name: "LocatedUnableGainInstitutionalAccess" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup LocatedWitdrawnFromProject = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_6", codeSystem: CodeSystems.Obhita, name: "LocatedWitdrawnFromProject" ),
            value: 6,
            sortOrder: 6
            );

        public static readonly Lookup UnableToLocateMoved = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_7", codeSystem: CodeSystems.Obhita, name: "UnableToLocateMoved" ),
            value: 7,
            sortOrder: 7
            );

        public static readonly Lookup UnableToLocateOther = new Lookup (
            codedConcept: new CodedConcept ( code: "S00000_8", codeSystem: CodeSystems.Obhita, name: "UnableToLocateOther" ),
            value: 7,
            sortOrder: 7
            );

        public static readonly List<Lookup> FollowUpStatus = new List<Lookup>()
                                                             {
                                                                 Deceased,
                                                                 CompletedInSpecifiedWindow,
                                                                 CompletedOutSpecifiedWindow,
                                                                 LocatedButRefused,
                                                                 LocatedUnableGainInstitutionalAccess,
                                                                 LocatedUnableGainAccess,
                                                                 LocatedWitdrawnFromProject,
                                                                 UnableToLocateMoved,
                                                                 UnableToLocateOther
                                                             };

        #endregion
    }
}