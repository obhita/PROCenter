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
using System.Data;
using System.Linq;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class RelyingPartyRepository : IRelyingPartyRepository
    {
        public bool TryGet(string realm, out RelyingParty relyingParty)
        {
            relyingParty = null;

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var match = (from rp in entities.RelyingParties
                             where rp.Realm.Equals(realm, StringComparison.OrdinalIgnoreCase) &&
                                   rp.Enabled == true
                             orderby rp.Realm descending
                             select rp)
                            .FirstOrDefault();

                if (match != null)
                {
                    relyingParty = match.ToDomainModel();
                    return true;
                }
            }

            return false;
        }

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<RelyingParty> List(int pageIndex, int pageSize)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var rps = from e in entities.RelyingParties
                          orderby e.Name
                          select e;

                if (pageIndex != -1 && pageSize != -1)
                {
                    rps = rps.Skip(pageIndex * pageSize).Take(pageSize).OrderBy(rp => rp.Name);
                }

                return rps.ToList().ToDomainModel();
            }
        }

        public RelyingParty Get(string id)
        {
            var uniqueId = int.Parse(id);

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return
                    (from rp in entities.RelyingParties
                     where rp.Id == uniqueId
                     select rp)
                    .First()
                    .ToDomainModel();
            }
        }

        public void Add(RelyingParty relyingParty)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                entities.RelyingParties.Add(relyingParty.ToEntity());
                entities.SaveChanges();
            }
        }

        public void Update(RelyingParty relyingParty)
        {
            var rpEntity = relyingParty.ToEntity();

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                entities.RelyingParties.Attach(rpEntity);
                entities.Entry(rpEntity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var rpEntity = new RelyingParties { Id = int.Parse(id) };
                entities.RelyingParties.Attach(rpEntity);
                entities.Entry(rpEntity).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }
        #endregion
    }
}
