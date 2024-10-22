﻿#region License Header

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

namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using System.Linq;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Domain.SecurityModule;
    using ProCenter.Domain.SecurityModule.Event;
    using ProCenter.Infrastructure.EventStore;

    #endregion

    /// <summary>The system account repository class.</summary>
    public class SystemAccountRepository : RepositoryBase<SystemAccount>, ISystemAccountRepository
    {
        #region Fields

        private readonly IEventStoreFactory _eventStoreFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="SystemAccountRepository" /> class.</summary>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        /// <param name="eventStoreFactory">The event store factory.</param>
        public SystemAccountRepository ( IUnitOfWorkProvider unitOfWorkProvider, IEventStoreFactory eventStoreFactory )
            : base ( unitOfWorkProvider )
        {
            _eventStoreFactory = eventStoreFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the by identifier.</summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A <see cref="SystemAccount" />.</returns>
        public SystemAccount GetByIdentifier ( string identifier )
        {
            var eventStore = _eventStoreFactory.Build<SystemAccount> ();
            var commits = eventStore.Advanced.GetFrom ( DateTime.MinValue );
            var systemAccountCreatedEvent = commits.SelectMany ( c => c.Events )
                .FirstOrDefault ( e => e.Body is SystemAccountCreatedEvent && ( e.Body as SystemAccountCreatedEvent ).Identifier == identifier );
            return systemAccountCreatedEvent == null ? null : EventStoreRepository.GetByKey<SystemAccount> ( ( systemAccountCreatedEvent.Body as SystemAccountCreatedEvent ).Key );

            //TODO: See if there is a way to do this without hitting the event store twice.
        }

        #endregion
    }
}