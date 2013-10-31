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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class OpenIdConnectClientsRepository : IOpenIdConnectClientsRepository
    {
        public bool ValidateClient(string clientId, string clientSecret)
        {
            Models.OpenIdConnectClient client;
            return ValidateClient(clientId, clientSecret, out client);

        }

        public bool ValidateClient(string clientId, string clientSecret, out Models.OpenIdConnectClient client)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = entities.OpenIdConnectClients.Find(clientId);
                if (record != null)
                {
                    if (Thinktecture.IdentityServer.Helper.CryptoHelper.VerifyHashedPassword(record.ClientSecret, clientSecret))
                    {
                        client = record.ToDomainModel();
                        return true;
                    }
                }
                
                client = null;
                return false;
            }
        }

        public IEnumerable<Models.OpenIdConnectClient> GetAll()
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return entities.OpenIdConnectClients.ToArray().Select(x => x.ToDomainModel()).ToArray();
            }
        }

        public Models.OpenIdConnectClient Get(string clientId)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.OpenIdConnectClients.Include("RedirectUris").Where(x=>x.ClientId==clientId).SingleOrDefault();
                if (item != null) return item.ToDomainModel();
            }

            return null;
        }

        public void Delete(string clientId)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.OpenIdConnectClients.Find(clientId);
                if (item != null)
                {
                    entities.OpenIdConnectClients.Remove(item);
                    entities.SaveChanges();
                }
            }
        }

        public void Update(Models.OpenIdConnectClient model)
        {
            if (model == null) throw new ArgumentNullException("model");
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.OpenIdConnectClients.Find(model.ClientId);
                if (item != null)
                {
                    model.UpdateEntity(item);
                    entities.SaveChanges();
                }
            }
        }

        public void Create(Models.OpenIdConnectClient model)
        {
            if (model == null) throw new ArgumentNullException("model");
            var item = new OpenIdConnectClientEntity();
            model.UpdateEntity(item);
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                entities.OpenIdConnectClients.Add(item);
                entities.SaveChanges();
            }
        }
    }
}
