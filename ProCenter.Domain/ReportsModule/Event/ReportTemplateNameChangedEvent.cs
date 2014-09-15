namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template name changed event class.</summary>
    public class ReportTemplateNameChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTemplateNameChangedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        public ReportTemplateNameChangedEvent ( Guid key, int version, string name )
            : base ( key, version )
        {
            Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        #endregion
    }
}