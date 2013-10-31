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
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class InitialConfigurationModel
    {
        [Display(Name = "SiteName", ResourceType = typeof(Resources.InitialConfigurationModel))]
        [Required]
        public string SiteName { get; set; }

        [Display(Name = "IssuerUri", ResourceType = typeof(Resources.InitialConfigurationModel))]
        [Required]
        public string IssuerUri { get; set; }

        [Display(Name = "SigningCertificate", ResourceType = typeof(Resources.InitialConfigurationModel))]
        [Required]
        public string SigningCertificate { get; set; }

        public List<string> AvailableCertificates { get; set; }

        [Display(Name = "CreateDefaultAccounts", ResourceType = typeof(Resources.InitialConfigurationModel))]
        public bool CreateDefaultAccounts { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resources.InitialConfigurationModel))]
        public string UserName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.InitialConfigurationModel))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<SelectListItem> AvailableCertificatesList
        {
            get
            {
                if (AvailableCertificates != null)
                {
                    return
                        (from c in AvailableCertificates
                         select new SelectListItem
                         {
                             Text = c,
                             Value = c
                         })
                        .ToList();
                }

                return null;
            }
        }
    }
}