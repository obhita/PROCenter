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
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class IdentityProviderRepository : IIdentityProviderRepository
    {
        IEnumerable<Models.IdentityProvider> IIdentityProviderRepository.GetAll()
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return entities.IdentityProviders.ToList().ToDomainModel();
            }
        }

        public bool TryGet(string name, out Models.IdentityProvider identityProvider)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                identityProvider = entities.IdentityProviders.Where(idp => idp.Name == name).FirstOrDefault().ToDomainModel();
                return (identityProvider != null);
            }
        }

        public void Add(Models.IdentityProvider item)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                ValidateUniqueName(item, entities);
                var entity = item.ToEntity();
                entities.IdentityProviders.Add(entity);
                entities.SaveChanges();
                item.ID = entity.ID;
            }
        }

        private static void ValidateUniqueName(Models.IdentityProvider item, IdentityServerConfigurationContext entities)
        {
            var othersWithSameName =
                from e in entities.IdentityProviders
                where e.Name == item.Name && e.ID != item.ID
                select e;
            if (othersWithSameName.Any()) throw new ValidationException(string.Format(Core.Repositories.Resources.IdentityProviderRepository.NameAlreadyInUseError, item.Name));
        }

        public void Delete(int id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.IdentityProviders.Where(idp => idp.ID == id).FirstOrDefault();
                if (item != null)
                {
                    entities.IdentityProviders.Remove(item);
                    entities.SaveChanges();
                }
            }
        }

        public void Update(Models.IdentityProvider item)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                ValidateUniqueName(item, entities);

                var dbitem = entities.IdentityProviders.Where(idp => idp.ID == item.ID).FirstOrDefault();
                if (dbitem != null)
                {
                    item.UpdateEntity(dbitem);
                    entities.SaveChanges();
                }
            }
        }


        public Models.IdentityProvider Get(int id)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.IdentityProviders.SingleOrDefault(x=>x.ID == id);
                if (item != null)
                {
                    return item.ToDomainModel();
                }
                return null;
            }
        }
    }
}
