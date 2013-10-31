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
    using System.Collections.Generic;
    using CommonModule;
    using CommonModule.Lookups;

    public class Frequency
    {
        public static readonly Lookup Never = new Lookup
            (
                codedConcept: new CodedConcept(code: "C25515_0", codeSystem: CodeSystems.Nci, name: "Never"),
                value : 0,
                sortOrder : 0
            );

        public static readonly Lookup DailyOrAlmostDaily = new Lookup
            (
                codedConcept :
                    new CodedConcept(code: "C25515_1", codeSystem: CodeSystems.Nci, name: "DailyorAlmostDaily"),
                value : 1,
                sortOrder : 1
            );

        public static readonly Lookup Weekly = new Lookup
            (
                codedConcept: new CodedConcept(code: "C25515_2", codeSystem: CodeSystems.Nci, name: "Weekly"),
                value: 2,
                sortOrder: 2
            );

        public static readonly Lookup MonthlyOrLessOften = new Lookup
            (
                codedConcept :
                    new CodedConcept(code: "C25515_3", codeSystem: CodeSystems.Nci, name: "MonthlyorLessOften"),
                value : 3,
                sortOrder : 3
            );

        public static readonly Lookup InThePast90Days = new Lookup
            (
                codedConcept:
                    new CodedConcept(code: "C25515_4", codeSystem: CodeSystems.Nci, name: "Inthepast90days"),
                value: 4,
                sortOrder: 4
            );

        public static readonly Lookup InThePastYear = new Lookup
            (
                codedConcept: new CodedConcept(code: "C25515_5", codeSystem: CodeSystems.Nci, name: "Inthepastyear"),
                value: 5,
                sortOrder: 5
            );

        public static readonly Lookup OverAYearAgo = new Lookup
            (
                codedConcept: new CodedConcept(code: "C25515_6", codeSystem: CodeSystems.Nci, name: "Overayearago"),
                value: 6,
                sortOrder: 6
            );

        public static readonly List<Lookup> DrugUseFrequencies = new List<Lookup>()
            {
                 Never,
                DailyOrAlmostDaily,
                Weekly,
                MonthlyOrLessOften
               
            };

        public static readonly List<Lookup> InjectionFrequencies = new List<Lookup>()
            { 
                InThePast90Days,
                InThePastYear,
                OverAYearAgo,
            };
    }
}