namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.MessageModule;

    #endregion

    public class WorkflowMessageRepository : RepositoryBase<WorkflowMessage>, IWorkflowMessageRepository
    {
        #region Constructors and Destructors

        public WorkflowMessageRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}