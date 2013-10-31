namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Class for defining gender lookups.
    /// </summary>
    public class Gender : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem GenderCodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     Female = 2.
        /// </summary>
        public static readonly Gender Female = new Gender
            {
                CodedConcept = new CodedConcept ( code: "Female", codeSystem: GenderCodeSystem, name: "Female" ),
                SortOrder = 1,
                Value = 2
            };

        /// <summary>
        ///     Male = 1.
        /// </summary>
        public static readonly Gender Male = new Gender
            {
                CodedConcept = new CodedConcept ( code: "Male", codeSystem: GenderCodeSystem, name: "Male" ),
                SortOrder = 0,
                Value = 1
            };

        #endregion
    }
}