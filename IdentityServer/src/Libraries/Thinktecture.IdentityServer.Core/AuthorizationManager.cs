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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public AuthorizationManager()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public AuthorizationManager(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public override bool CheckAccess(AuthorizationContext context)
        {
            var action = context.Action.First();
            var id = context.Principal.Identities.First();

            // if application authorization request
            if (action.Type.Equals(ClaimsAuthorization.ActionType))
            {
                return AuthorizeCore(action, context.Resource, context.Principal.Identity as ClaimsIdentity);
            }

            // if ws-trust issue request
            if (action.Value.Equals(WSTrust13Constants.Actions.Issue))
            {
                return AuthorizeTokenIssuance(new Collection<Claim> { new Claim(ClaimsAuthorization.ResourceType, Constants.Resources.WSTrust) }, id);
            }

            return base.CheckAccess(context);
        }

        protected virtual bool AuthorizeCore(Claim action, Collection<Claim> resource, ClaimsIdentity id)
        {
            switch (action.Value)
            {
                case Constants.Actions.Issue:
                    return AuthorizeTokenIssuance(resource, id);
                case Constants.Actions.Administration:
                    return AuthorizeAdministration(resource, id);
                case Constants.Actions.WebApi:
                    return AuthorizeWebApi(resource, id);
            }

            return false;
        }

        protected virtual bool AuthorizeTokenIssuance(Collection<Claim> resource, ClaimsIdentity id)
        {
            if (!ConfigurationRepository.Global.EnforceUsersGroupMembership)
            {
                var authResult = id.IsAuthenticated;
                if (!authResult)
                {
                    Tracing.Error("Authorization for token issuance failed because the user is anonymous");
                }

                return authResult;
            }

            var roleResult = id.HasClaim(ClaimTypes.Role, Constants.Roles.IdentityServerUsers);
            if (!roleResult)
            {
                Tracing.Error(string.Format("Authorization for token issuance failed because user {0} is not in the {1} role", id.Name, Constants.Roles.IdentityServerUsers));
            }

            return roleResult;
        }

        protected virtual bool AuthorizeAdministration(Collection<Claim> resource, ClaimsIdentity id)
        {
            var roleResult = id.HasClaim(ClaimTypes.Role, Constants.Roles.IdentityServerAdministrators);
            if (!roleResult)
            {
                if (resource[0].Value != Constants.Resources.UI)
                {
                    Tracing.Error(string.Format("Administration authorization failed because user {0} is not in the {1} role", id.Name, Constants.Roles.IdentityServerAdministrators));
                }
            }

            return roleResult;
        }

        protected virtual bool AuthorizeWebApi(Collection<Claim> resource, ClaimsIdentity id)
        {
            var roleResult = id.HasClaim(ClaimTypes.Role, Constants.Roles.WebApi);
            if (!roleResult)
            {
                if (resource[0].Value != Constants.Resources.UI)
                {
                    Tracing.Error(string.Format("Web Api authorization failed because user {0} is not in the {1} role", id.Name, Constants.Roles.WebApi));
                }
            }

            return roleResult;
        }
    }
}