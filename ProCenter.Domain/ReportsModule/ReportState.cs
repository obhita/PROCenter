namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The report state class.</summary>
    public class ReportState : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem _codeSystem = CodeSystems.Obhita;

        public static readonly ReportState Deleted = new ReportState
                                                     {
                                                         CodedConcept = new CodedConcept ( code: "Deleted", codeSystem: _codeSystem, name: "Deleted" ),
                                                         SortOrder = 1,
                                                         Value = 1
                                                     };

        public static readonly ReportState Normal = new ReportState
                                                    {
                                                        CodedConcept = new CodedConcept ( code: "Normal", codeSystem: _codeSystem, name: "Normal" ),
                                                        SortOrder = 0,
                                                        Value = 0
                                                    };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportState"/> class.
        /// </summary>
        protected internal ReportState ()
        {
        }

        #endregion
    }
}