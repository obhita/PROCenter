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
namespace ProCenter.Infrastructure.Domain
{
    #region Using Statements

    using System;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Infrastructure.Service;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    ///     Factory for building aggregates.
    /// </summary>
    public class AggregateFactory : IAggregateFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds an aggregate for the specified key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="memento">The memento.</param>
        /// <returns>An aggregate.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public TAggregate Build<TAggregate> ( Guid key, IMemento memento )
            where TAggregate : class, IAggregateRoot
        {
            var aggregateType = typeof(TAggregate);
            var jsonSerializerSettings = new JsonSerializerSettings {ContractResolver = new AllowPrivateSetterContractResolver ()};
            jsonSerializerSettings.Converters.Add ( new AggregateConverter () );

            var aggregate = JsonConvert.DeserializeObject<TAggregate> ( string.Format ( "{{\"Key\":\"{0}\"}}", key ), jsonSerializerSettings);
            if ( memento != null )
            {
                aggregate.RestoreSnapshot ( memento );
            }

            var keySetter = aggregateType.GetProperty ( "Key" ).GetSetMethod (true);
            keySetter.Invoke ( aggregate, new object[] { key } );

            return aggregate;
        }

        #endregion

    }
}