namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.OrganizationModule;

    #endregion

    /// <summary>
    ///     Organization repository.
    /// </summary>
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationRepository" /> class.
        /// </summary>
        /// <param name="eventStoreRepository">The event store repository.</param>
        public OrganizationRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}