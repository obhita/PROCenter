namespace ProCenter.Domain.AssessmentModule
{
    /// <summary>
    /// Interface for indicating this generates a report.
    /// </summary>
    public interface IGenerateReport
    {
        /// <summary>
        /// Gets the severity.
        /// </summary>
        /// <value>
        /// The severity.
        /// </value>
        ReportSeverity Severity { get; }
    }
}
