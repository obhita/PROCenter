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