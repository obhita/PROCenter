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
namespace Thinktecture.IdentityServer
{
    public static class Constants
    {
        public const string AuthenticationType = "IdentityServer";
        public const string InternalIssuer = "IdentityServerInternal";

        public static class Claims
        {
            public const string Base = "http://identityserver.thinktecture.com/claims/";

            public const string IdentityProvider = Base + "identityprovider";
            public const string Client = Base + "client";
            public const string Scope = Base + "scope";
        }

        public static class Actions
        {
            public const string Issue          = "Issue";
            public const string Administration = "Administration";
            public const string WebApi = "WebApi";
        }

        public static class Resources
        {
            // issue actions
            public const string WSFederation = "WSFederation";
            public const string SimpleHttp   = "SimpleHttp";
            public const string OAuth2       = "OAuth2";
            public const string WRAP         = "WRAP";
            public const string WSTrust      = "WSTrust";
            public const string JSNotify     = "JSNotify";

            // administration actions
            public const string General             = "General";
            public const string Configuration       = "Configuration";
            public const string RelyingParty        = "RelyingParty";
            public const string ServiceCertificates = "ServiceCertificates";
            public const string ClientCertificates  = "ClientCertificates";
            public const string Delegation          = "Delegation";
            public const string UI                  = "UI";
        }

        public static class Roles
        {
            public const string InternalRolesPrefix          = "IdentityServer";
            public const string Users                        = "Users";
            public const string Administrators               = "Administrators";

            public const string IdentityServerUsers          = InternalRolesPrefix + Users;
            public const string IdentityServerAdministrators = InternalRolesPrefix + Administrators;

            public const string WebApi = "WebApi";
        }

        public static class CacheKeys
        {
            public const string WSFedMetadata = "Cache_WSFedMetadata";
        }
    }
}
