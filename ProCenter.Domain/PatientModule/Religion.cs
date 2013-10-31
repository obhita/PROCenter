namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    /// Class for defining religion lookups.
    /// </summary>
    public class Religion : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem ReligionCodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     Catholic = 2.
        /// </summary>
        public static readonly Religion Catholic = new Religion
            {
                CodedConcept = new CodedConcept ( code: "Catholic", codeSystem: ReligionCodeSystem, name: "Catholic" ),
                SortOrder = 2,
                Value = 2
            };

        /// <summary>
        ///     Islamic = 4.
        /// </summary>
        public static readonly Religion Islamic = new Religion
            {
                CodedConcept = new CodedConcept ( code: "Islamic", codeSystem: ReligionCodeSystem, name: "Islamic" ),
                SortOrder = 4,
                Value = 4
            };

        /// <summary>
        ///     Jewish = 3.
        /// </summary>
        public static readonly Religion Jewish = new Religion
            {
                CodedConcept = new CodedConcept ( code: "Jewish", codeSystem: ReligionCodeSystem, name: "Jewish" ),
                SortOrder = 3,
                Value = 3
            };

        /// <summary>
        ///     None = 6.
        /// </summary>
        public static readonly Religion None = new Religion
            {
                CodedConcept = new CodedConcept ( code: "None", codeSystem: ReligionCodeSystem, name: "None" ),
                SortOrder = 6,
                Value = 6
            };

        /// <summary>
        ///     Other = 5.
        /// </summary>
        public static readonly Religion Other = new Religion
            {
                CodedConcept = new CodedConcept ( code: "Other", codeSystem: ReligionCodeSystem, name: "Other" ),
                SortOrder = 5,
                Value = 5
            };

        /// <summary>
        ///     Protestant = 1.
        /// </summary>
        public static readonly Religion Protestant = new Religion
            {
                CodedConcept = new CodedConcept ( code: "Protestant", codeSystem: ReligionCodeSystem, name: "Protestant" ),
                SortOrder = 1,
                Value = 1
            };

        #endregion
    }
}