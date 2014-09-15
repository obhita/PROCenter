namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>Interface for report template factory.</summary>
    public interface IReportTemplateFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="name">The name.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="reportState">State of the report.</param>
        /// <returns>
        /// A <see cref="ReportTemplate" />.
        /// </returns>
        ReportTemplate Create ( Guid staffKey, string name, ReportType reportType, object parameters, ReportState reportState );

        #endregion
    }
}