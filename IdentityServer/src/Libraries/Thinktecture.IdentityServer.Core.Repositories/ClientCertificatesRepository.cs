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
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClientCertificatesRepository : IClientCertificatesRepository
    {
        #region Runtime
        public bool TryGetUserNameFromThumbprint(X509Certificate2 certificate, out string userName)
        {
            userName = null;

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                userName = (from mapping in entities.ClientCertificates
                            where mapping.Thumbprint.Equals(certificate.Thumbprint, StringComparison.OrdinalIgnoreCase)
                            select mapping.UserName).FirstOrDefault();

                return (userName != null);
            }
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<string> List(int pageIndex, int pageSize)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var users =
                    (from user in entities.ClientCertificates
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

        public IEnumerable<ClientCertificate> GetClientCertificatesForUser(string userName)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var certs =
                     from record in entities.ClientCertificates
                     where record.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                     select record;

                return certs.ToList().ToDomainModel();
            }
        }

        public void Add(ClientCertificate certificate)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record =
                    (from entry in entities.ClientCertificates
                     where entry.UserName.Equals(certificate.UserName, StringComparison.OrdinalIgnoreCase) &&
                           entry.Thumbprint.Equals(certificate.Thumbprint, StringComparison.OrdinalIgnoreCase)
                     select entry)
                    .SingleOrDefault();
                if (record == null)
                {
                    record = new ClientCertificates
                    {
                        UserName = certificate.UserName,
                        Thumbprint = certificate.Thumbprint,
                    };
                    entities.ClientCertificates.Add(record);
                }
                record.Description = certificate.Description;
                entities.SaveChanges();
            }
        }

        public void Delete(ClientCertificate certificate)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record =
                    (from entry in entities.ClientCertificates
                     where entry.UserName.Equals(certificate.UserName, StringComparison.OrdinalIgnoreCase) &&
                           entry.Thumbprint.Equals(certificate.Thumbprint, StringComparison.OrdinalIgnoreCase)
                     select entry)
                    .SingleOrDefault();
                if (record != null)
                {
                    entities.ClientCertificates.Remove(record);
                    entities.SaveChanges();
                }
            }
        }
        #endregion
    }
}
