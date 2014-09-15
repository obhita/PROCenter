namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using ProCenter.Domain.ReportsModule;

    #endregion

    /// <summary>The recent report repository class.</summary>
    public class RecentReportRepository : RepositoryBase<RecentReport>, IRecentReportRepository
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="RecentReportRepository" /> class.</summary>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public RecentReportRepository(IUnitOfWorkProvider unitOfWorkProvider)
            : base ( unitOfWorkProvider )
        {
        }

        #endregion
    }
}