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

    /// <summary>The professional information lookups class.</summary>
    public class ProfessionalInformationLookups
    {
        #region Static Fields

        public static readonly Lookup BachelorsDegree = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_16", codeSystem: CodeSystems.Obhita, name: "BachelorsDegree" ),
            value: 16,
            sortOrder: 16
            );

        public static readonly Lookup CollegeFirstYear = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_13", codeSystem: CodeSystems.Obhita, name: "CollegeFirstYear" ),
            value: 13,
            sortOrder: 13
            );

        public static readonly Lookup CollegeSecondYear = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_14", codeSystem: CodeSystems.Obhita, name: "CollegeSecondYear" ),
            value: 14,
            sortOrder: 14
            );

        public static readonly Lookup CollegeThirdYear = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_15", codeSystem: CodeSystems.Obhita, name: "CollegeThirdYear" ),
            value: 15,
            sortOrder: 15
            );

        public static readonly Lookup EigthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_8", codeSystem: CodeSystems.Obhita, name: "EigthGrade" ),
            value: 8,
            sortOrder: 8
            );

        public static readonly Lookup EleventhGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_11", codeSystem: CodeSystems.Obhita, name: "EleventhGrade" ),
            value: 11,
            sortOrder: 11
            );

        public static readonly Lookup EmployedFullTime = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_0", codeSystem: CodeSystems.Obhita, name: "EmployedFullTime" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup EmployedPartTime = new Lookup (
            codedConcept:
                new CodedConcept ( code: "L00000_1", codeSystem: CodeSystems.Obhita, name: "EmployedPartTime" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup EmploymentOther = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_7", codeSystem: CodeSystems.Obhita, name: "EmploymentOther" ),
            value: 7,
            sortOrder: 7
            );

        public static readonly Lookup FifthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_5", codeSystem: CodeSystems.Obhita, name: "FifthGrade" ),
            value: 5,
            sortOrder: 5
            );

        public static readonly Lookup FirstGrade = new Lookup (
            codedConcept:
                new CodedConcept ( code: "K00000_1", codeSystem: CodeSystems.Obhita, name: "FirstGrade" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup FourthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_4", codeSystem: CodeSystems.Obhita, name: "FourthGrade" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup FullTime = new Lookup (
            codedConcept:
                new CodedConcept ( code: "J00000_1", codeSystem: CodeSystems.Obhita, name: "FullTime" ),
            value: 1,
            sortOrder: 1
            );

        public static readonly Lookup NeverAttended = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_0", codeSystem: CodeSystems.Obhita, name: "NeverAttended" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup NinthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_9", codeSystem: CodeSystems.Obhita, name: "NinthGrade" ),
            value: 9,
            sortOrder: 9
            );

        public static readonly Lookup NotEnrolled = new Lookup (
            codedConcept: new CodedConcept ( code: "J00000_0", codeSystem: CodeSystems.Obhita, name: "NotEnrolled" ),
            value: 0,
            sortOrder: 0
            );

        public static readonly Lookup Other = new Lookup (
            codedConcept: new CodedConcept ( code: "J00000_3", codeSystem: CodeSystems.Obhita, name: "Other" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup PartTime = new Lookup (
            codedConcept: new CodedConcept ( code: "J00000_2", codeSystem: CodeSystems.Obhita, name: "PartTime" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup SecondGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_2", codeSystem: CodeSystems.Obhita, name: "SecondGrade" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup SeventhGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_7", codeSystem: CodeSystems.Obhita, name: "SeventhGrade" ),
            value: 7,
            sortOrder: 7
            );

        public static readonly Lookup SixthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_6", codeSystem: CodeSystems.Obhita, name: "SixthGrade" ),
            value: 6,
            sortOrder: 6
            );

        public static readonly Lookup TenthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_10", codeSystem: CodeSystems.Obhita, name: "TenthGrade" ),
            value: 10,
            sortOrder: 10
            );

        public static readonly Lookup ThirdGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_3", codeSystem: CodeSystems.Obhita, name: "ThirdGrade" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup TwelfthGrade = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_12", codeSystem: CodeSystems.Obhita, name: "TwelfthGrade" ),
            value: 12,
            sortOrder: 12
            );

        public static readonly Lookup UnemployedDisabled = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_3", codeSystem: CodeSystems.Obhita, name: "UnemployedDisabled" ),
            value: 3,
            sortOrder: 3
            );

        public static readonly Lookup UnemployedLooking = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_2", codeSystem: CodeSystems.Obhita, name: "UnemployedLooking" ),
            value: 2,
            sortOrder: 2
            );

        public static readonly Lookup UnemployedNotLooking = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_6", codeSystem: CodeSystems.Obhita, name: "UnemployedNotLooking" ),
            value: 6,
            sortOrder: 6
            );

        public static readonly Lookup UnemployedRetired = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_5", codeSystem: CodeSystems.Obhita, name: "UnemployedRetired" ),
            value: 5,
            sortOrder: 5
            );

        public static readonly Lookup UnemployedVolunteerWork = new Lookup (
            codedConcept: new CodedConcept ( code: "L00000_4", codeSystem: CodeSystems.Obhita, name: "UnemployedVolunteerWork" ),
            value: 4,
            sortOrder: 4
            );

        public static readonly Lookup VocationNoDiploma = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_17", codeSystem: CodeSystems.Obhita, name: "VocationNoDiploma" ),
            value: 17,
            sortOrder: 17
            );

        public static readonly Lookup VocationWithDiploma = new Lookup (
            codedConcept: new CodedConcept ( code: "K00000_18", codeSystem: CodeSystems.Obhita, name: "VocationWithDiploma" ),
            value: 18,
            sortOrder: 18
            );

        public static readonly List<Lookup> EducationLevel = new List<Lookup>()
                                                             {
                                                                 NeverAttended,
                                                                 FirstGrade,
                                                                 SecondGrade,
                                                                 ThirdGrade,
                                                                 FourthGrade,
                                                                 FifthGrade,
                                                                 SixthGrade,
                                                                 SeventhGrade,
                                                                 EigthGrade,
                                                                 NinthGrade,
                                                                 TenthGrade,
                                                                 EleventhGrade,
                                                                 TwelfthGrade,
                                                                 CollegeFirstYear,
                                                                 CollegeSecondYear,
                                                                 CollegeThirdYear,
                                                                 VocationNoDiploma,
                                                                 VocationWithDiploma
                                                             };

        public static readonly List<Lookup> EmploymentStatus = new List<Lookup>()
                                                               {
                                                                   EmployedFullTime,
                                                                   EmployedPartTime,
                                                                   UnemployedLooking,
                                                                   UnemployedDisabled,
                                                                   UnemployedVolunteerWork,
                                                                   UnemployedRetired,
                                                                   UnemployedNotLooking,
                                                                   EmploymentOther
                                                               };

        public static readonly List<Lookup> JobTrainingEnrollment = new List<Lookup>()
                                                                    {
                                                                        NotEnrolled,
                                                                        FullTime,
                                                                        PartTime,
                                                                        Other
                                                                    };

        #endregion
    }
}