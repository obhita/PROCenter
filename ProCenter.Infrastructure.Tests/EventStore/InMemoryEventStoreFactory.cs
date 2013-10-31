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