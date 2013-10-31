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
using System.ComponentModel.Composition;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UsersViewModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        private Repositories.IUserManagementRepository UserManagementRepository;

        public UsersViewModel(Repositories.IUserManagementRepository UserManagementRepository, string filter)
        {
            Container.Current.SatisfyImportsOnce(this);

            this.UserManagementRepository = UserManagementRepository;
            this.Filter = filter;

            if (String.IsNullOrEmpty(filter))
            {
                Users = UserManagementRepository.GetUsers();
                Total = Showing = Users.Count();
            }
            else
            {
                Users = UserManagementRepository.GetUsers(filter);
                Total = UserManagementRepository.GetUsers().Count();
                Showing = Users.Count();
            }

            UsersDeleteList = Users.Select(x => new UserDeleteModel { Username = x }).ToArray();
        }

        public IEnumerable<string> Users { get; set; }
        public UserDeleteModel[] UsersDeleteList {get;set;}
        public string Filter { get; set; }
        public int Total { get; set; }
        public int Showing { get; set; }

        public bool IsProfileEnabled
        {
            get
            {
                return System.Web.Profile.ProfileManager.Enabled;
            }
        }
        
        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return ConfigurationRepository.OAuth2.Enabled &&
                    (ConfigurationRepository.OAuth2.EnableCodeFlow || ConfigurationRepository.OAuth2.EnableResourceOwnerFlow);
            }
        }
    }

    public class UserDeleteModel
    {
        public string Username { get; set; }
        public bool Delete { get; set; }
    }
}