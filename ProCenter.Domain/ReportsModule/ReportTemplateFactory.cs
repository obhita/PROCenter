namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The report template factory class.</summary>
    public class ReportTemplateFactory : IReportTemplateFactory
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
        public ReportTemplate Create (Guid staffKey, string name, ReportType reportType, object parameters, ReportState reportState )
        {
            return new ReportTemplate ( staffKey, name, reportType, parameters, reportState );
        }

        #endregion
    }
}