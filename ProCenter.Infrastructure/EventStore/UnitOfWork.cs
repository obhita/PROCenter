#region License Header

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

    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Implements the unit of work for a request.</summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Fields

        private readonly Guid _commitId = CombGuid.NewCombGuid ();

        private readonly IEventStoreRepository _eventStoreRepository;

        private readonly Dictionary<IAggregateRoot, List<IDomainEvent>> _uncommitedEvents = new Dictionary<IAggregateRoot, List<IDomainEvent>> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="UnitOfWork" /> class.</summary>
        /// <param name="eventStoreRepository">The repository.</param>
        public UnitOfWork ( IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the event store repository.</summary>
        /// <value>The event store repository.</value>
        public IEventStoreRepository EventStoreRepository
        {
            get { return _eventStoreRepository; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Commits the unit of work.
        /// </summary>
        public void Commit ()
        {
            foreach ( var uncommitedEvents in _uncommitedEvents )
            {
                _eventStoreRepository.Save ( uncommitedEvents.Key, uncommitedEvents.Value, _commitId, a => { } );
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

        /// <summary>
        ///     Gets an aggregate for the specified key.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of <see cref="IAggregateRoot" />.
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     A <see cref="IAggregateRoot" />.
        /// </returns>
        public T Get<T> ( Guid key ) where T : class, IAggregateRoot
        {
            return _uncommitedEvents.Keys.FirstOrDefault ( a => a.Key == key ) as T;
        }

        /// <summary>
        ///     Registers the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="events">The events.</param>
        public void Register ( IAggregateRoot aggregateRoot, params IDomainEvent[] events )
        {
            if ( _uncommitedEvents.ContainsKey ( aggregateRoot ) )
            {
                _uncommitedEvents[aggregateRoot].AddRange ( events );
            }
            else
            {
                _uncommitedEvents.Add ( aggregateRoot, new List<IDomainEvent> ( events ) );
            }
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

            if ( _eventStoreRepository is IDisposable )
            {
                ( _eventStoreRepository as IDisposable ).Dispose ();
            }
        }

        #endregion
    }
}