namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Utility;
    using PipelineHook;
    using Service.ReadSideService;
    using global::EventStore;
    using global::EventStore.Dispatcher;

    #endregion

    /// <summary>
    ///     Factory for building event stores based on aggregate types.
    /// </summary>
    public class EventStoreFactory : IEventStoreFactory
    {
        private readonly IDispatchCommits _commitDispatcher;

        #region Fields

        private readonly Dictionary<Type, IStoreEvents> _eventStores = new Dictionary<Type, IStoreEvents>();

        #endregion

        public EventStoreFactory(IDispatchCommits commitDispatcher)
        {
            _commitDispatcher = commitDispatcher;
        }

        #region Public Methods and Operators

        /// <summary>
        /// Builds the event store for aggregates of type <typeparam name="TAggregate"></typeparam>.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <returns>
        /// An <see cref="IStoreEvents" />.
        /// </returns>
        public IStoreEvents Build<TAggregate>()
        {
            return Build(typeof (TAggregate));
        }

        /// <summary>
        ///     Builds the event store for the specified aggregate type.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <returns>
        ///     An <see cref="IStoreEvents" />.
        /// </returns>
        public IStoreEvents Build(Type aggregateType)
        {
            Check.IsNotNull(aggregateType, "Aggregate type cannot be null.");

            lock (_eventStores)
            {
                if (!_eventStores.ContainsKey(aggregateType))
                {
                    //TODO: Add configuration for compression/encryption parameters
                    var eventStore = Wireup.Init()
                                           .LogToOutputWindow()
                                           .UsingRavenPersistence("RavenDbPROCenter").Partition(aggregateType.Name)
                                           .ConsistentQueries()
                                           .InitializeStorageEngine()
                                           .UsingAsynchronousDispatchScheduler()
                                           .DispatchTo(_commitDispatcher)
                                           .HookIntoPipelineUsing(new AuditPipelineHook(null))
                                           .Build();
                    _eventStores.Add(aggregateType, eventStore);
                }
                return _eventStores[aggregateType];
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            lock (_eventStores)
            {
                foreach (var eventStore in _eventStores)
                    eventStore.Value.Dispose();

                _eventStores.Clear();
            }
        }

        #endregion
    }
}