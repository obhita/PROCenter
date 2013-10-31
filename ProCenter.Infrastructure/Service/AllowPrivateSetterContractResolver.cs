namespace ProCenter.Infrastructure.Service
{
    #region Using Statements

    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    #endregion

    /// <summary>
    ///     Allows serialization of private settable properties.
    /// </summary>
    public class AllowPrivateSetterContractResolver : DefaultContractResolver
    {
        // more @ http://daniel.wertheim.se/2010/11/06/json-net-private-setters/

        #region Methods

        /// <summary>
        ///     Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
        ///     <see
        ///         cref="T:System.Reflection.MemberInfo" />
        ///     .
        /// </summary>
        /// <param name="member">
        ///     The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.
        /// </param>
        /// <param name="memberSerialization">
        ///     The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.
        /// </param>
        /// <returns>
        ///     A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given
        ///     <see
        ///         cref="T:System.Reflection.MemberInfo" />
        ///     .
        /// </returns>
        protected override JsonProperty CreateProperty ( MemberInfo member, MemberSerialization memberSerialization )
        {
            var prop = base.CreateProperty ( member, memberSerialization );

            if ( !prop.Writable )
            {
                var property = member as PropertyInfo;

                if ( property != null )
                {
                    var hasPrivateSetter = property.GetSetMethod ( true ) != null;

                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }

        #endregion
    }
}