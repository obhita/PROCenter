using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProCenter.Infrastructure.EventStore
{
    using System.IO;

    using NEventStore;
    using NEventStore.Serialization;

    using Newtonsoft.Json;

    using ProCenter.Infrastructure.Service;

    /// <summary>
    /// The class for ProCenterJsonSerializer.
    /// </summary>
    public class ProCenterJsonSerializer : ISerialize
    {
        private readonly IEnumerable<Type> _knownTypes = new[]
                                                         {
                                                             typeof(List<EventMessage>),
                                                             typeof(Dictionary<string, object>)
                                                         };

        private readonly Newtonsoft.Json.JsonSerializer _typedSerializer = new Newtonsoft.Json.JsonSerializer ()
                                                                           {
                                                                               TypeNameHandling = TypeNameHandling.All,
                                                                               DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                               NullValueHandling = NullValueHandling.Ignore,
                                                                               ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                                                                               ContractResolver = new AllowPrivateSetterContractResolver ()
                                                                           };

        private readonly Newtonsoft.Json.JsonSerializer _untypedSerializer = new Newtonsoft.Json.JsonSerializer ()
                                                                             {
                                                                                 TypeNameHandling = TypeNameHandling.Auto,
                                                                                 DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                 NullValueHandling = NullValueHandling.Ignore,
                                                                                 ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                                                                                 ContractResolver = new AllowPrivateSetterContractResolver()
                                                                             };

        /// <summary>
        /// Initializes a new instance of the <see cref="ProCenterJsonSerializer"/> class.
        /// </summary>
        /// <param name="knownTypes">The known types.</param>
        public ProCenterJsonSerializer ( params Type[] knownTypes )
        {
            if ( knownTypes != null )
            {
                _knownTypes = _knownTypes.Union ( knownTypes );
            }
        }

        /// <summary>
        /// Serializes the specified output.
        /// </summary>
        /// <typeparam name="T">The type of the graph.</typeparam>
        /// <param name="output">The output.</param>
        /// <param name="graph">The graph.</param>
        public virtual void Serialize<T> ( Stream output, T graph )
        {
            using ( var streamWriter = new StreamWriter ( output, Encoding.UTF8 ) )
            {
                Serialize ( new JsonTextWriter ( streamWriter ), graph );
            }
        }

        /// <summary>
        /// Deserializes the specified input.
        /// </summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="input">The input.</param>
        /// <returns>Returns a deserialized type.</returns>
        public virtual T Deserialize<T> ( Stream input )
        {
            using ( var streamReader = new StreamReader ( input, Encoding.UTF8 ) )
            {
                return Deserialize<T> ( new JsonTextReader ( streamReader ) );
            }
        }

        /// <summary>
        /// Serializes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="graph">The graph.</param>
        protected virtual void Serialize ( JsonWriter writer, object graph )
        {
            using ( writer )
            {
                GetSerializer ( graph.GetType () ).Serialize ( writer, graph );
            }
        }

        /// <summary>
        /// Deserializes the specified reader.
        /// </summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns>Returns a deserialized type.</returns>
        protected virtual T Deserialize<T> ( JsonReader reader )
        {
            var type = typeof(T);
            using ( reader )
            {
                return (T)GetSerializer ( type ).Deserialize ( reader, type );
            }
        }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <param name="typeToSerialize">The type to serialize.</param>
        /// <returns>Returns an instance of the serializer.</returns>
        protected virtual Newtonsoft.Json.JsonSerializer GetSerializer ( Type typeToSerialize )
        {
            if ( _knownTypes.Contains ( typeToSerialize ) )
            {
                return _untypedSerializer;
            }
            return _typedSerializer;
        }
    }
}