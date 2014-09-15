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

namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NEventStore;
    using NEventStore.Dispatcher;

    using Pillar.Common.Utility;

    using ProCenter.Infrastructure.EventStore.PipelineHook;

    #endregion

    /// <summary>Factory for building event stores based on aggregate types.</summary>
    public class EventStoreFactory : IEventStoreFactory
    {
        #region Fields

        private readonly IDispatchCommits _commitDispatcher;

        private readonly Dictionary<Type, IStoreEvents> _eventStores = new Dictionary<Type, IStoreEvents> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventStoreFactory" /> class.
        /// </summary>
        /// <param name="commitDispatcher">The commit dispatcher.</param>
        public EventStoreFactory ( IDispatchCommits commitDispatcher )
        {
            _commitDispatcher = commitDispatcher;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the cached event stores.
        /// </summary>
        /// <value>
        ///     The cached event stores.
        /// </value>
        public IReadOnlyCollection<IStoreEvents> CachedEventStores
        {
            get { return _eventStores.Values.ToList ().AsReadOnly (); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Builds the event store for aggregates of type
        ///     <typeparam name="TAggregate"></typeparam>
        ///     .
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <returns>
        ///     An <see cref="IStoreEvents" />.
        /// </returns>
        public IStoreEvents Build<TAggregate> ()
        {
            return Build ( typeof(TAggregate) );
        }

        /// <summary>
        ///     Builds the event store for the specified aggregate type.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <returns>
        ///     An <see cref="IStoreEvents" />.
        /// </returns>
        public IStoreEvents Build ( Type aggregateType )
        {
            Check.IsNotNull ( aggregateType, "Aggregate type cannot be null." );

            lock ( _eventStores )
            {
                if ( !_eventStores.ContainsKey ( aggregateType ) )
                {
                    //TODO: Add configuration for compression/encryption parameters
                    var eventStore = Wireup.Init ()
                        .LogToOutputWindow ()
                        .UsingRavenPersistence ( "RavenDbPROCenter" ).Partition ( aggregateType.Name )
                        .ConsistentQueries ()
                        .InitializeStorageEngine ()
                        .UsingCustomSerialization ( new ProCenterJsonSerializer () )
                        .UsingAsynchronousDispatchScheduler ( _commitDispatcher )
                        .HookIntoPipelineUsing ( new AuditPipelineHook ( null ) )
                        .Build ();
                    _eventStores.Add ( aggregateType, eventStore );
                }
                return _eventStores[aggregateType];
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposing )
            {
                return;
            }

            lock ( _eventStores )
            {
                foreach ( var eventStore in _eventStores )
                {
                    eventStore.Value.Dispose ();
                }

                _eventStores.Clear ();
            }
        }

        #endregion
    }
}