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
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DelegationRepository : IDelegationRepository
    {
        #region Runtime
        public bool IsDelegationAllowed(string userName, string realm)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = (from entry in entities.Delegation
                              where entry.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                    entry.Realm.Equals(realm, StringComparison.OrdinalIgnoreCase)
                              select entry).FirstOrDefault();

                return (record != null);
            }
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<string> GetAllUsers(int pageIndex, int pageSize)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var users =
                    (from user in entities.Delegation
                     orderby user.UserName
                     select user.UserName)
                    .Distinct();

                if (pageIndex != -1 && pageSize != -1)
                {
                    users = users.Skip(pageIndex * pageSize).Take(pageSize);
                }

                return users.ToList();
            }
        }

        public IEnumerable<DelegationSetting> GetDelegationSettingsForUser(string userName)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var settings =
                     from record in entities.Delegation
                     where record.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                     select record;

                return settings.ToList().ToDomainModel();
            }
        }

        public void Add(DelegationSetting setting)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var entity = new Delegation
                {
                    UserName = setting.UserName,
                    Realm = setting.Realm.AbsoluteUri,
                    Description = setting.Description
                };

                entities.Delegation.Add(entity);
                entities.SaveChanges();
            }
        }

        public void Delete(DelegationSetting setting)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record =
                    (from entry in entities.Delegation
                     where entry.UserName.Equals(setting.UserName, StringComparison.OrdinalIgnoreCase) &&
                           entry.Realm.Equals(setting.Realm.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                     select entry)
                    .Single();

                entities.Delegation.Remove(record);
                entities.SaveChanges();
            }
        }
        #endregion
    }
}
