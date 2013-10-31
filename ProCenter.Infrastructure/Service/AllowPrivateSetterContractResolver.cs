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