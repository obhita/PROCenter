namespace ProCenter.Infrastructure.Tests.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Infrastructure.EventStore;
    using global::EventStore;

    #endregion

    public class InMemoryEventStoreFactory : IEventStoreFactory
    {
        #region Fields

        private readonly Dictionary<Type, IStoreEvents> _eventStores = new Dictionary<Type, IStoreEvents>();

        #endregion

        #region Public Methods and Operators

        public IStoreEvents Build<TAggregate>()
        {
            return Build(typeof (TAggregate));
        }


        public IStoreEvents Build(Type aggregateType)
        {
            lock (_eventStores)
            {
                if (!_eventStores.ContainsKey(aggregateType))
                {
                    var eventStore = Wireup.Init()
                                           .UsingInMemoryPersistence()
                                           .InitializeStorageEngine()
                                           .Build();
                    _eventStores.Add(aggregateType, eventStore);
                }
                return _eventStores[aggregateType];
            }
        }


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