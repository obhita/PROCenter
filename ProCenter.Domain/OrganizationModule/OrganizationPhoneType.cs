namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Organization Phone Type
    /// </summary>
    public class OrganizationPhoneType : Lookup
    {
        #region Static Fields

        /// <summary>
        ///     Office = 0.
        /// </summary>
        public static readonly OrganizationPhoneType Office = new OrganizationPhoneType
            {
                CodedConcept = new CodedConcept ( code: "Office", codeSystem: CodeSystem, name: "Office" ),
                SortOrder = 1,
                Value = 0
            };

        private static readonly CodeSystem CodeSystem = CodeSystems.Obhita;

        #endregion
    }
}