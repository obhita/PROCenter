namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using ProCenter.Domain.ReportsModule;

    #endregion

    /// <summary>The report template repository class.</summary>
    public class ReportTemplateRepository : RepositoryBase<ReportTemplate>, IReportTemplateRepository
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ReportTemplateRepository" /> class.</summary>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public ReportTemplateRepository(IUnitOfWorkProvider unitOfWorkProvider)
            : base ( unitOfWorkProvider )
        {
        }

        #endregion
    }
}