namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.OrganizationModule;

    #endregion

    public class StaffRepository : RepositoryBase<Staff>, IStaffRepository
    {
        #region Constructors and Destructors

        public StaffRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}