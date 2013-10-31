namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.OrganizationModule;

    #endregion

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        #region Constructors and Destructors

        public TeamRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}