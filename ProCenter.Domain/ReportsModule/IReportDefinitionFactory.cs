namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>Interface for report template factory.</summary>
    public interface IReportDefinitionFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the specified report definition.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isPatientCentric">If set to <c>true</c> [is patient centric].</param>
        /// <returns>
        /// A <see cref="ReportTemplate" />.
        /// </returns>
        ReportDefinition Create ( Guid staffKey, string reportName, string displayName, bool isPatientCentric );

        #endregion
    }
}