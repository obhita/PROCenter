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
using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public static class OidcConstants
    {
        static Dictionary<string, IEnumerable<string>> _dictionary = new Dictionary<string, IEnumerable<string>>
        {
            { Scopes.Profile, new string[]
                            { 
                                ClaimTypes.Name,
                                ClaimTypes.FamilyName,
                                ClaimTypes.GivenName,
                                ClaimTypes.MiddleName,
                                ClaimTypes.NickName,
                                ClaimTypes.PreferredUserName,
                                ClaimTypes.Profile,
                                ClaimTypes.Picture,
                                ClaimTypes.WebSite,
                                ClaimTypes.Gender,
                                ClaimTypes.BirthDate,
                                ClaimTypes.ZoneInfo,
                                ClaimTypes.Locale,
                                ClaimTypes.UpdatedAt 
                            }},
            { Scopes.Email, new string[]
                            { 
                                ClaimTypes.Email,
                                ClaimTypes.EmailVerified 
                            }},
            { Scopes.Address, new string[]
                            {
                                ClaimTypes.Address
                            }},
            { Scopes.Phone, new string[]
                            {
                                ClaimTypes.PhoneNumber,
                                ClaimTypes.PhoneNumberVerified
                            }},
        };

        public static Dictionary<string, IEnumerable<string>> Mappings 
        {
            get { return _dictionary; } 
        }

        public static class Scopes
        {
            public const string OpenId        = "openid";
            public const string Profile       = "profile";
            public const string Email         = "email";
            public const string Address       = "address";
            public const string Phone         = "phone";
            public const string OfflineAccess = "offline_access";
        }

        public static class ClaimTypes
        {
            public const string Subject             = "sub";
            public const string Name                = "name";
            public const string GivenName           = "given_name";
            public const string FamilyName          = "family_name";
            public const string MiddleName          = "middle_name";
            public const string NickName            = "nickname";
            public const string PreferredUserName   = "preferred_username";
            public const string Profile             = "profile";
            public const string Picture             = "picture";
            public const string WebSite             = "website";
            public const string Email               = "email";
            public const string EmailVerified       = "email_verified";
            public const string Gender              = "gender";
            public const string BirthDate           = "birthdate";
            public const string ZoneInfo            = "zoneinfo";
            public const string Locale              = "locale";
            public const string PhoneNumber         = "phone_number";
            public const string PhoneNumberVerified = "phone_number_verified";
            public const string Address             = "address";
            public const string UpdatedAt           = "updated_at";
        }
    }
}
