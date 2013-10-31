#region Using Statements

#endregion

namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Class for defining ethnicity lookups
    /// </summary>
    public class Ethnicity : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem EthnicityCodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     AfricanAmerican = 0.
        /// </summary>
        public static readonly Ethnicity AfricanAmerican = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "AfricanAmerican", codeSystem: EthnicityCodeSystem, name: "AfricanAmerican" ),
                SortOrder = 3,
                Value = 2
            };

        /// <summary>
        ///     AlaskanNative = 0.
        /// </summary>
        public static readonly Ethnicity AlaskanNative = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "AlaskanNative", codeSystem: EthnicityCodeSystem, name: "AlaskanNative" ),
                SortOrder = 6,
                Value = 5
            };

        /// <summary>
        ///     Asian = 0.
        /// </summary>
        public static readonly Ethnicity Asian = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "Asian", codeSystem: EthnicityCodeSystem, name: "Asian" ),
                SortOrder = 8,
                Value = 7
            };

        /// <summary>
        ///     Caucasian = 0.
        /// </summary>
        public static readonly Ethnicity Caucasian = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "Caucasian", codeSystem: EthnicityCodeSystem, name: "Caucasian" ),
                SortOrder = 2,
                Value = 1
            };

        /// <summary>
        ///     Hispanic = 0.
        /// </summary>
        public static readonly Ethnicity Hispanic = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "Hispanic", codeSystem: EthnicityCodeSystem, name: "Hispanic" ),
                SortOrder = 7,
                Value = 6
            };

        /// <summary>
        ///     NativeAmerican = 0.
        /// </summary>
        public static readonly Ethnicity NativeAmerican = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "NativeAmerican", codeSystem: EthnicityCodeSystem, name: "NativeAmerican" ),
                SortOrder = 4,
                Value = 3
            };

        /// <summary>
        ///     PacificIslander = 0.
        /// </summary>
        public static readonly Ethnicity PacificIslander = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "PacificIslander", codeSystem: EthnicityCodeSystem, name: "PacificIslander" ),
                SortOrder = 5,
                Value = 4
            };

        /// <summary>
        ///     Undeclared = 0.
        /// </summary>
        public static readonly Ethnicity Undeclared = new Ethnicity
            {
                CodedConcept = new CodedConcept ( code: "Undeclared", codeSystem: EthnicityCodeSystem, name: "Undeclared" ),
                SortOrder = 1,
                Value = 0
            };

        #endregion
    }
}