namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template parameters changed event class.</summary>
    public class ReportTemplateParametersChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateParametersChangedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="parameters">The parameters.</param>
        public ReportTemplateParametersChangedEvent ( Guid key, int version, object parameters )
            : base ( key, version )
        {
            Parameters = parameters;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public object Parameters { get; private set; }

        #endregion
    }
}