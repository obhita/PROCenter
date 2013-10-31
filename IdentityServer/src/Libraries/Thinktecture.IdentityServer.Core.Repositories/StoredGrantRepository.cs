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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class StoredGrantRepository : IStoredGrantRepository
    {
        public void Add(Models.StoredGrant grant)
        {
            var entity = grant.ToEntityModel();

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                entities.StoredGrants.Add(entity);
                entities.SaveChanges();
            }
        }

        public Models.StoredGrant Get(string id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var result = (from sg in entities.StoredGrants
                              where sg.GrantId == id
                              select sg)
                             .SingleOrDefault();

                if (result != null)
                {
                    return result.ToDomainModel();
                }

                return null;
            }
        }

        public void Delete(string id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.StoredGrants.Where(x => x.GrantId.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (item != null)
                {
                    entities.StoredGrants.Remove(item);
                    entities.SaveChanges();
                }
            }
        }
    }
}
