namespace ProCenter.Domain.CommonModule
{
    /// <summary>
    ///     Static class containing common code systems.
    /// </summary>
    public static class CodeSystems
    {
        #region Static Fields

        /// <summary>
        /// The obhita code system.
        /// </summary>
        public static CodeSystem Obhita = new CodeSystem ( code: "", version:"", name:"OBHITA" );

        /// <summary>
        ///     The loinc code system.
        /// </summary>
        public static CodeSystem Loinc = new CodeSystem ( code: "2.16.840.1.113883.6.1", version: "", name: "LOINC" );

        /// <summary>
        ///     The national cancer institute code system.
        /// </summary>
        public static CodeSystem Nci = new CodeSystem ( code: "2.16.840.1.113883.3.26", version: "", name: "NationalCancerInstitute" );

        /// <summary>
        ///     The snomed CT code system.
        /// </summary>
        public static CodeSystem SnomedCT = new CodeSystem ( code: "2.16.840.1.113883.6.96", version: "", name: "SNOMEDCT" );

        #endregion
    }
}