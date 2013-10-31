#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Pillar.Domain.Event;
    using PipelineHook;
    using ProCenter.Domain.CommonModule;
    using global::EventStore;
    using global::EventStore.Persistence;

    #endregion

    /// <summary>
    ///     Event store repository.
    /// </summary>
    public class EventStoreRepository : IEventStoreRepository, IDisposable
    {
        #region Constants

        private const string AggregateTypeHeader = "AggregateType";

        #endregion

        #region Fields

        private readonly IDetectConflicts _conflictDetector;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IEventStoreFactory _eventStoreFactory;
        private readonly IAggregateFactory _factory;
        private readonly IDictionary<Guid, Snapshot> _snapshots = new Dictionary<Guid, Snapshot> ();
        private readonly IDictionary<Guid, IEventStream> _streams = new Dictionary<Guid, IEventStream> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreRepository" /> class.
        /// </summary>
        /// <param name="eventStoreFactory">The event store factory.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="conflictDetector">The conflict detector.</param>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public EventStoreRepository (
            IEventStoreFactory eventStoreFactory,
            IAggregateFactory factory,
            IDetectConflicts conflictDetector, 
            IUnitOfWorkProvider unitOfWorkProvider )
        {
            _eventStoreFactory = eventStoreFactory;
            _factory = factory;
            _conflictDetector = conflictDetector;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The aggregate key.</param>
        /// <returns>
        /// Last modified date.
        /// </returns>
        public virtual DateTime? GetLastModifiedDate<TAggregate>(Guid key)
        {
            var eventStore = _eventStoreFactory.Build<TAggregate>();
            var snapshot = GetSnapshot(eventStore, key, int.MaxValue);
            var stream = OpenStream(eventStore, key, int.MaxValue, snapshot);
            var times = stream.CommittedHeaders.Where ( kvp => kvp.Key == AuditPipelineHook.TimestampHeader ).Select ( kvp => DateTime.Parse(kvp.Value.ToString ()) );
            if ( times.Any () )
            {
                return times.Max ();
            }
            return null;
        }

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TAggregate GetByKey<TAggregate> ( Guid key )
            where TAggregate : class, IAggregateRoot
        {
            return GetByKey<TAggregate> ( key, int.MaxValue );
        }

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="versionToLoad">The version to load.</param>
        /// <returns></returns>
        public virtual TAggregate GetByKey<TAggregate> ( Guid key, int versionToLoad )
            where TAggregate : class, IAggregateRoot
        {
            var unitOfWork = _unitOfWorkProvider.GetCurrentUnitOfWork();
            if (unitOfWork != null)
            {
                var cachedAggregate = unitOfWork.Get<TAggregate>(key);
                if (cachedAggregate != null)
                {
                    return cachedAggregate;
                }
            }
            var eventStore = _eventStoreFactory.Build<TAggregate> ();
            var snapshot = GetSnapshot(eventStore, key, versionToLoad);
            var stream = OpenStream(eventStore, key, versionToLoad, snapshot);
            var aggregate = GetAggregate<TAggregate> ( snapshot, stream );

            ApplyEventsToAggregate ( versionToLoad, stream, aggregate );

            return aggregate as TAggregate;
        }

        /// <summary>
        ///     Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="uncommitedEvents">The uncommited events.</param>
        /// <param name="commitId">The commit id.</param>
        /// <param name="updateHeaders">The update headers.</param>
        /// <exception cref="ProCenter.Infrastructure.EventStore.ConflictingCommandException"></exception>
        /// <exception cref="ProCenter.Infrastructure.EventStore.PersistenceException"></exception>
        public virtual void Save ( IAggregateRoot aggregate, IEnumerable<IDomainEvent> uncommitedEvents, Guid commitId, Action<IDictionary<string, object>> updateHeaders )
        {
            var headers = PrepareHeaders ( aggregate, updateHeaders );
            var eventStore = _eventStoreFactory.Build ( aggregate.GetType () );
            while ( true )
            {
                var stream = PrepareStream ( eventStore, aggregate, headers, uncommitedEvents );
                var commitEventCount = stream.CommittedEvents.Count;

                try
                {
                    stream.CommitChanges ( commitId );
                    return;
                }
                catch ( DuplicateCommitException )
                {
                    stream.ClearChanges ();
                    return;
                }
                catch ( ConcurrencyException e )
                {
                    if ( this.ThrowOnConflict ( stream, commitEventCount ) )
                        throw new ConflictingCommandException ( e.Message, e );

                    stream.ClearChanges ();
                }
                catch ( StorageException e )
                {
                    throw new PersistenceException ( e.Message, e );
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposing )
                return;

            lock ( _streams )
            {
                foreach ( var stream in _streams )
                    stream.Value.Dispose ();

                _snapshots.Clear ();
                _streams.Clear ();
            }
        }

        private static void ApplyEventsToAggregate ( int versionToLoad, IEventStream stream, IAggregateRoot aggregate )
        {
            if ( versionToLoad == 0 || aggregate.Version < versionToLoad )
                foreach ( var @event in stream.CommittedEvents.Select ( x => x.Body ) )
                    aggregate.ApplyEvent ( @event );
        }

        private static Dictionary<string, object> PrepareHeaders ( IAggregateRoot aggregate, Action<IDictionary<string, object>> updateHeaders )
        {
            var headers = new Dictionary<string, object> ();

            headers[AggregateTypeHeader] = aggregate.GetType ().FullName;
            if ( updateHeaders != null )
                updateHeaders ( headers );

            return headers;
        }

        private IAggregateRoot GetAggregate<TAggregate> ( Snapshot snapshot, IEventStream stream )
            where TAggregate : class, IAggregateRoot
        {
            var memento = snapshot == null ? null : snapshot.Payload as IMemento;
            return this._factory.Build<TAggregate> ( stream.StreamId, memento );
        }

        private Snapshot GetSnapshot ( IStoreEvents eventStore, Guid id, int version )
        {
            Snapshot snapshot;
            if ( !_snapshots.TryGetValue ( id, out snapshot ) )
                _snapshots[id] = snapshot = eventStore.Advanced.GetSnapshot(id, version);

            return snapshot;
        }

        private IEventStream OpenStream(IStoreEvents eventStore, Guid id, int version, Snapshot snapshot)
        {
            IEventStream stream;
            if ( _streams.TryGetValue ( id, out stream ) )
                return stream;

            stream = snapshot == null
                         ? eventStore.OpenStream ( id, 0, version )
                         : eventStore.OpenStream ( snapshot, version );

            return this._streams[id] = stream;
        }

        private IEventStream PrepareStream( IStoreEvents eventStore, IAggregateRoot aggregate, Dictionary<string, object> headers, IEnumerable<IDomainEvent> uncommitedEvents)
        {
            IEventStream stream;
            if ( !_streams.TryGetValue ( aggregate.Key, out stream ) )
                _streams[aggregate.Key] = stream = eventStore.CreateStream ( aggregate.Key );

            foreach ( var item in headers )
                stream.UncommittedHeaders[item.Key] = item.Value;

            uncommitedEvents
                .Cast<object> ()
                .Select ( x => new EventMessage {Body = x} )
                .ToList ()
                .ForEach ( stream.Add );

            return stream;
        }

        private bool ThrowOnConflict ( IEventStream stream, int skip )
        {
            var committed = stream.CommittedEvents.Skip ( skip ).Select ( x => x.Body );
            var uncommitted = stream.UncommittedEvents.Select ( x => x.Body );
            return _conflictDetector.ConflictsWith ( uncommitted, committed );
        }

        #endregion
    }
}