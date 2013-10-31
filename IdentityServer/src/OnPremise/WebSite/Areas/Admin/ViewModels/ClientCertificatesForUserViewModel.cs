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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ClientCertificatesForUserViewModel
    {
        private Repositories.IClientCertificatesRepository clientCertificatesRepository;
        private Repositories.IUserManagementRepository userManagementRepository;
        
        [Required]
        public string UserName { get; set; }
        public IEnumerable<ClientCertificate> Certificates { get; set; }

        public ClientCertificate NewCertificate { get; set; }

        public bool IsNew
        {
            get
            {
                return this.UserName == null;
            }
        }

        public IEnumerable<SelectListItem> AllUserNames { get; set; }

        public ClientCertificatesForUserViewModel(IClientCertificatesRepository clientCertificatesRepository, IUserManagementRepository userManagementRepository, string username)
        {
            this.clientCertificatesRepository = clientCertificatesRepository;
            this.userManagementRepository = userManagementRepository;
            var allnames =
                userManagementRepository.GetUsers()
                .Select(x => new SelectListItem
                {
                    Text = x
                }).ToList();
            allnames.Insert(0, new SelectListItem { Text = Resources.ClientCertificatesForUserViewModel.ChooseItem, Value = "" });
            this.AllUserNames = allnames;
            
            this.UserName = username;
            NewCertificate = new ClientCertificate { UserName = username };
            if (!IsNew)
            {
                var certs =
                        this.clientCertificatesRepository
                        .GetClientCertificatesForUser(this.UserName)
                            .ToArray();
                this.Certificates = certs;
            }
            else
            {
                this.Certificates = new ClientCertificate[0];
            }
        }
    }
}