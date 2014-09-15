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

    using Pillar.Domain.Event;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Interface for repository.</summary>
    public interface IEventStoreRepository
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The aggregate.</returns>
        TAggregate GetByKey<TAggregate> ( Guid key ) where TAggregate : class, IAggregateRoot;

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <returns>The aggregate.</returns>
        TAggregate GetByKey<TAggregate> ( Guid key, int version ) where TAggregate : class, IAggregateRoot;

        /// <summary>
        ///     Gets the last modified date.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The aggregate key.</param>
        /// <returns>
        ///     Last modified date.
        /// </returns>
        DateTime? GetLastModifiedDate<TAggregate> ( Guid key );

        /// <summary>
        ///     Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="uncommitedEvents">The uncommited events.</param>
        /// <param name="commitId">The commit id.</param>
        /// <param name="updateHeaders">The update headers.</param>
        void Save ( IAggregateRoot aggregate, IEnumerable<IDomainEvent> uncommitedEvents, Guid commitId, Action<IDictionary<string, object>> updateHeaders );

        #endregion
    }
}