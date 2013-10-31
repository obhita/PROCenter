namespace ProCenter.Domain.AssessmentModule.Lookups
{
    #region Using Statements

    using System.Linq;
    using System.Collections;
    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Class for defining item type lookups.
    /// </summary>
    public class ItemType : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem ObhitaCodeSystem = CodeSystems.Obhita;
        
        /// <summary>
        ///     Question = 0.
        /// </summary>
        public static readonly ItemType Question = new ItemType
            {
                CodedConcept = new CodedConcept(code: "Question", codeSystem: ObhitaCodeSystem, name: "Question"),
                SortOrder = 0,
                Value = 0
            };
        
        /// <summary>
        ///     Group = 1.
        /// </summary>
        public static readonly ItemType Group = new ItemType
            {
                CodedConcept = new CodedConcept(code: "Group", codeSystem: ObhitaCodeSystem, name: "Group"),
                SortOrder = 0,
                Value = 1
            };
        
        /// <summary>
        ///     Section = 2.
        /// </summary>
        public static readonly ItemType Section = new ItemType
            {
                CodedConcept = new CodedConcept(code: "Section", codeSystem: ObhitaCodeSystem, name: "Section"),
                SortOrder = 0,
                Value = 2
            };

        #endregion
    }
}