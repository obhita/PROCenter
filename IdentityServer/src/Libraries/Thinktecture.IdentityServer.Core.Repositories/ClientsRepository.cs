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
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClientsRepository : IClientsRepository
    {
        public bool ValidateClient(string clientId, string clientSecret)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = (from c in entities.Clients
                              where c.ClientId.Equals(clientId, StringComparison.Ordinal)
                              select c).SingleOrDefault();
                if (record != null)
                {
                    return Thinktecture.IdentityServer.Helper.CryptoHelper.VerifyHashedPassword(record.ClientSecret, clientSecret);
                }
                return false;
            }
        }

        public bool TryGetClient(string clientId, out Models.Client client)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = (from c in entities.Clients
                              where c.ClientId.Equals(clientId, StringComparison.Ordinal)
                              select c).SingleOrDefault();

                if (record != null)
                {
                    client = record.ToDomainModel();
                    return true;
                }

                client = null;
                return false;
            }
        }

        public bool ValidateAndGetClient(string clientId, string clientSecret, out Models.Client client)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = (from c in entities.Clients
                              where c.ClientId.Equals(clientId, StringComparison.Ordinal)
                              select c).SingleOrDefault();
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
        
        public IEnumerable<Models.Client> GetAll()
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return entities.Clients.ToArray().Select(x => x.ToDomainModel()).ToArray();
            }
        }


        public void Delete(int id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.Clients.Where(x => x.Id == id).SingleOrDefault();
                if (item != null)
                {
                    entities.Clients.Remove(item);
                    entities.SaveChanges();
                }
            }
        }
        public void Update(Models.Client model)
        {
            if (model == null) throw new ArgumentException("model");

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.Clients.Where(x => x.Id == model.ID).Single();
                model.UpdateEntity(item);
                entities.SaveChanges();
            }
        }

        public void Create(Models.Client model)
        {
            if (model == null) throw new ArgumentException("model");

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = new Client();
                model.UpdateEntity(item);
                entities.Clients.Add(item);
                entities.SaveChanges();
                model.ID = item.Id;
            }
        }


        public Models.Client Get(int id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.Clients.Where(x => x.Id == id).SingleOrDefault();
                if (item != null)
                {
                    return item.ToDomainModel();
                }
                return null;
            }
        }
    }
}
