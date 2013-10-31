namespace ProCenter.Infrastructure.Service
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Converters;
    using ProCenter.Domain.CommonModule;

    public class AggregateConverter : CustomCreationConverter<IAggregateRoot>
    {
        public override IAggregateRoot Create ( Type objectType )
        {
            var aggregateRoot = Activator.CreateInstance ( objectType );
            return (IAggregateRoot)aggregateRoot;
        }
    }
}