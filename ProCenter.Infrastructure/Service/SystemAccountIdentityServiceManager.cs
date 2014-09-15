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

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ProCenter.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.IdentityModel.Services;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Pillar.Common.Configuration;

    #endregion

    /// <summary>Manager for handling Identity server account functions.</summary>
    public class SystemAccountIdentityServiceManager : ISystemAccountIdentityServiceManager
    {
        #region Constants

        private const string IdentityServerPassword = "IdentityServerWebApiPassword";

        private const string IdentityServerUserName = "IdentityServerWebApiUsername";

        #endregion

        #region Fields

        private readonly Uri _baseAddress;

        private readonly string _password;

        private readonly string _userName;

        private DateTime _expiration;

        private string _token;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountIdentityServiceManager" /> class.
        /// </summary>
        /// <param name="appSettingsConfiguration">The app settings configuration.</param>
        public SystemAccountIdentityServiceManager ( IConfigurationPropertiesProvider appSettingsConfiguration )
        {
            var issuerUri = new Uri ( ( FederatedAuthentication.WSFederationAuthenticationModule ?? new WSFederationAuthenticationModule () ).Issuer );
            var url = issuerUri.AbsoluteUri.Replace ( "issue/wsfed", string.Empty );
            if ( url.EndsWith ( "//" ) )
            {
                url = url.Substring ( 0, url.Length - 1 );
            }
            _baseAddress = new Uri ( url );
            _userName = appSettingsConfiguration.GetProperty<string> ( IdentityServerUserName );
            _password = appSettingsConfiguration.GetProperty<string> ( IdentityServerPassword );
            _expiration = DateTime.MinValue;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Changes the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The response from the identity service.</returns>
        public IdentityServiceResponse ChangePassword ( string email, string oldPassword, string newPassword )
        {
            var identityServiecResponse = new IdentityServiceResponse ();
            var response = MakeRequestAsync ( HttpMethod.Post, "api/membership/ChangePassword/" + email, new { oldPassword, newPassword } ).Result;
            if ( response.IsSuccessStatusCode )
            {
                identityServiecResponse.Sucess = true;
            }
            else
            {
                identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync ().Result;
            }
            return identityServiecResponse;
        }

        /// <summary>
        ///     Creates an account with the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        public IdentityServiceResponse Create ( string email )
        {
            var identityServiecResponse = new IdentityServiceResponse ();
            var response = MakeRequestAsync ( HttpMethod.Put, "api/membership/Create/" + email, new { email } ).Result;
            if ( response.IsSuccessStatusCode )
            {
                identityServiecResponse.Sucess = true;
            }
            else
            {
                identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync ().Result;
            }
            return identityServiecResponse;
        }

        /// <summary>
        ///     Locks the account for the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        public IdentityServiceResponse Lock ( string email )
        {
            var identityServiecResponse = new IdentityServiceResponse ();
            var response = MakeRequestAsync ( HttpMethod.Post, "api/membership/Lock/" + email ).Result;
            if ( response.IsSuccessStatusCode )
            {
                identityServiecResponse.Sucess = true;
            }
            else
            {
                identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync ().Result;
            }
            return identityServiecResponse;
        }

        /// <summary>
        ///     Resets the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        public IdentityServiceResponse ResetPassword ( string email )
        {
            var identityServiecResponse = new IdentityServiceResponse ();
            var response = MakeRequestAsync ( HttpMethod.Post, "api/membership/ResetPassword/" + email ).Result;
            if ( response.IsSuccessStatusCode )
            {
                identityServiecResponse.Sucess = true;
            }
            else
            {
                identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync ().Result;
            }
            return identityServiecResponse;
        }

        /// <summary>
        ///     Unlocks the account for the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        public IdentityServiceResponse UnLock ( string email )
        {
            var identityServiecResponse = new IdentityServiceResponse ();
            var response = MakeRequestAsync ( HttpMethod.Post, "api/membership/UnLock/" + email ).Result;
            if ( response.IsSuccessStatusCode )
            {
                identityServiecResponse.Sucess = true;
            }
            else
            {
                identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync ().Result;
            }
            return identityServiecResponse;
        }

        #endregion

        #region Methods

        private async Task<HttpResponseMessage> MakeRequestAsync ( HttpMethod method, string url, object data = null )
        {
            if ( _expiration <= DateTime.UtcNow )
            {
                var result = await RequestSessionToken ();
                if ( !result.IsSuccessStatusCode )
                {
                    return result;
                }
            }
            using ( var httpClient = new HttpClient { BaseAddress = _baseAddress } )
            {
                httpClient.SetToken ( "Session", _token );
                if ( method == HttpMethod.Get )
                {
                    return await httpClient.GetAsync ( url );
                }
                if ( method == HttpMethod.Post )
                {
                    var content = data == null ? string.Empty : JsonConvert.SerializeObject ( data );
                    return await httpClient.PostAsync ( url, new StringContent ( content ) );
                }
                if ( method == HttpMethod.Put )
                {
                    var content = data == null ? string.Empty : JsonConvert.SerializeObject ( data );
                    var response = await httpClient.PutAsync ( url, new StringContent ( content ) );
                    return response;
                }
                throw new ArgumentException ( "Invalid method: " + method, "method" );
            }
        }

        private async Task<HttpResponseMessage> RequestSessionToken ()
        {
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif          
            using ( var httpClient = new HttpClient { BaseAddress = _baseAddress } )
            {
                httpClient.SetBasicAuthentication ( _userName, _password );

                var response = await httpClient.GetAsync("issue/simple?realm=" + _baseAddress + "api/&tokenType=jwt");
                if ( !response.IsSuccessStatusCode )
                {
                    return response;
                }

                var tokenResponse = response.Content.ReadAsStringAsync ().Result;
                var json = JObject.Parse ( tokenResponse );
                _token = json["access_token"].ToString ();
                var expiresIn = int.Parse ( json["expires_in"].ToString () );
                _expiration = DateTime.UtcNow.AddSeconds ( expiresIn );

                return response;
            }
        }

        #endregion
    }
}