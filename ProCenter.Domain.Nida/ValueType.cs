namespace ProCenter.Domain.Nida
{
    using CommonModule;
    using CommonModule.Lookups;

    public static class ValueType
    {
        public static readonly Lookup Count = new Lookup (new CodedConcept(code: "C25463", codeSystem: CodeSystems.Nci, name: "Count"));

        public static readonly Lookup Score = new Lookup (new CodedConcept(code: "C25338", codeSystem: CodeSystems.Nci, name: "Score"));

        public static readonly Lookup YesOrNoResponse = new Lookup (new CodedConcept(code: "C38147", codeSystem: CodeSystems.Nci, name: "Yes or No Response"));

        public static readonly Lookup Frequency = new Lookup(new CodedConcept(code: "C25515", codeSystem: CodeSystems.Nci, name: "Frequency"));

        public static readonly Lookup Specify = new Lookup (new CodedConcept(code: "C25685", codeSystem: CodeSystems.Nci, name: "Specify"));
    }
}