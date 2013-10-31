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