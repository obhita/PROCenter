namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The report time period class.</summary>
    public class ReportTimePeriod : Lookup
    {
        #region Static Fields

        private static readonly CodeSystem _codeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     LastMonth = 0.
        /// </summary>
        public static readonly ReportTimePeriod LastMonth = new ReportTimePeriod
                                                            {
                                                                CodedConcept = new CodedConcept ( code: "LastMonth", codeSystem: _codeSystem, name: "LastMonth" ),
                                                                SortOrder = 0,
                                                                Value = 0
                                                            };

        /// <summary>
        ///     LastSixMonths = 2.
        /// </summary>
        public static readonly ReportTimePeriod LastSixMonths = new ReportTimePeriod
                                                                {
                                                                    CodedConcept = new CodedConcept ( code: "LastSixMonths", codeSystem: _codeSystem, name: "LastSixMonths" ),
                                                                    SortOrder = 2,
                                                                    Value = 2
                                                                };

        /// <summary>
        ///     LastThreeMonths = 1.
        /// </summary>
        public static readonly ReportTimePeriod LastThreeMonths = new ReportTimePeriod
                                                                  {
                                                                      CodedConcept = new CodedConcept ( code: "LastThreeMonths", codeSystem: _codeSystem, name: "LastThreeMonths" ),
                                                                      SortOrder = 1,
                                                                      Value = 1
                                                                  };

        /// <summary>
        ///     LastYear = 3.
        /// </summary>
        public static readonly ReportTimePeriod LastYear = new ReportTimePeriod
                                                           {
                                                               CodedConcept = new CodedConcept ( code: "LastYear", codeSystem: _codeSystem, name: "LastYear" ),
                                                               SortOrder = 3,
                                                               Value = 3
                                                           };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTimePeriod"/> class.
        /// </summary>
        protected internal ReportTimePeriod ()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the range.</summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public void GetRange ( out DateTime? start, out DateTime? end )
        {
            start = end = DateTime.Now;
            if ( this == LastMonth )
            {
                start = end.Value.AddMonths (-1 );
            }
            else if ( this == LastThreeMonths )
            {
                start = end.Value.AddMonths (-3 );
            }
            else if ( this == LastSixMonths )
            {
                start = end.Value.AddMonths (-6 );
            }
            else if ( this == LastYear )
            {
                start = end.Value.AddYears (-1 );
            }
        }

        #endregion
    }
}