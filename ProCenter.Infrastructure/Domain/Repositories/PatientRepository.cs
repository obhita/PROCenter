namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.PatientModule;

    #endregion

    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        #region Constructors and Destructors

        public PatientRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}