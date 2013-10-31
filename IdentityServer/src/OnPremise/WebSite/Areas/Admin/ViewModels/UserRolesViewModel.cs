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

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserRolesViewModel
    {
        private Repositories.IUserManagementRepository userManagementRepository;
        public string Username { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool IsUserInRole(string role)
        {
            return UserRoles.Contains(role);
        }
        public UserRoleAssignment[] RoleAssignments
        {
            get
            {
                var allRoles = this.userManagementRepository.GetRoles();
                return (from role in allRoles
                        select new UserRoleAssignment
                        {
                            Role = role,
                            InRole = IsUserInRole(role)
                        }).ToArray();
            }
        }

        public UserRolesViewModel(Repositories.IUserManagementRepository userManagementRepository, string username)
        {
            this.userManagementRepository = userManagementRepository;
            this.Username = username;
            this.UserRoles = this.userManagementRepository.GetRolesForUser(this.Username);
        }
    }

    public class UserRoleAssignment
    {
        [Required]
        public string Role { get; set; }
        public bool InRole { get; set; }
    }
}