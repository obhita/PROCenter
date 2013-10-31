namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.SecurityModule;

    #endregion

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        #region Constructors and Destructors

        public RoleRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}