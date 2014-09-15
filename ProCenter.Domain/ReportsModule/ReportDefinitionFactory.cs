namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The report definition factory class.</summary>
    public class ReportDefinitionFactory : IReportDefinitionFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isPatientCentric">If set to <c>true</c> [is patient centric].</param>
        /// <returns>
        /// A <see cref="ReportTemplate" />.
        /// </returns>
        public ReportDefinition Create (Guid staffKey, string reportName, string displayName, bool isPatientCentric )
        {
            return new ReportDefinition(staffKey, reportName, displayName, isPatientCentric);
        }

        #endregion
    }
}