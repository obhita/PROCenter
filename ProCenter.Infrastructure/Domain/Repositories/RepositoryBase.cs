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
namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using EventStore;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    /// Repository Base
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    public abstract class RepositoryBase<TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        #region Fields

        /// <summary>
        /// The _event store repository
        /// </summary>
        protected readonly IEventStoreRepository _eventStoreRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TAggregate}" /> class.
        /// </summary>
        /// <param name="eventStoreRepository">The event store repository.</param>
        protected RepositoryBase ( IEventStoreRepository eventStoreRepository )
        {
            _eventStoreRepository = eventStoreRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the aggregate by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TAggregate GetByKey ( Guid key )
        {
            return _eventStoreRepository.GetByKey<TAggregate> ( key );
        }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <param name="key">The aggregate key.</param>
        /// <returns>Last modified date.</returns>
        public DateTime? GetLastModifiedDate ( Guid key )
        {
            return _eventStoreRepository.GetLastModifiedDate<TAggregate> ( key );
        }

        #endregion
    }
}