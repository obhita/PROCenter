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
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web.Mvc;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class InitialConfigurationController : System.Web.Mvc.Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        [Import]
        public IUserManagementRepository UserManagement { get; set; }

        public InitialConfigurationController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public InitialConfigurationController(IConfigurationRepository configuration, IUserManagementRepository userManagement)
        {
            ConfigurationRepository = configuration;
            UserManagement = userManagement;
        }

        public ActionResult Index()
        {
            if (ConfigurationRepository.Keys.SigningCertificate != null)
            {
                return RedirectToAction("index", "home");
            }

            var model = new InitialConfigurationModel
            {
                AvailableCertificates = GetAvailableCertificatesFromStore(),
                IssuerUri = ConfigurationRepository.Global.IssuerUri,
                SiteName = ConfigurationRepository.Global.SiteName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InitialConfigurationModel model)
        {
            if (ConfigurationRepository.Keys.SigningCertificate != null)
            {
                return RedirectToAction("index", "home");
            }

            if (ModelState.IsValid)
            {
                var config = ConfigurationRepository.Global;
                config.SiteName = model.SiteName;
                config.IssuerUri = model.IssuerUri;

                // create default IdentityServer groups and admin user.
                if (model.CreateDefaultAccounts)
                {
                    var errors = CreateDefaultAccounts(model.UserName, model.Password);

                    if (errors.Count != 0)
                    {
                        errors.ForEach(e => ModelState.AddModelError("", e));
                        model.AvailableCertificates = GetAvailableCertificatesFromStore();
                        return View(model);
                    }
                }

                // update global config
                ConfigurationRepository.Global = config;

                var keys = ConfigurationRepository.Keys;
                try
                {
                    var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(model.SigningCertificate, false).First();
                    
                    // make sure we can access the private key
                    var pk = cert.PrivateKey;
                    
                    keys.SigningCertificate = cert;
                }
                catch (CryptographicException)
                {
                    ModelState.AddModelError("", string.Format(Resources.InitialConfigurationController.NoReadAccessPrivateKey, WindowsIdentity.GetCurrent().Name));
                    model.AvailableCertificates = GetAvailableCertificatesFromStore();
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(keys.SymmetricSigningKey))
                {
                    keys.SymmetricSigningKey = Convert.ToBase64String(CryptoRandom.CreateRandomKey(32));
                }
                
                // updates key material config
                ConfigurationRepository.Keys = keys;


                
                return RedirectToAction("index", "home");
            }

            ModelState.AddModelError("", Resources.InitialConfigurationController.ErrorsOcurred);
            model.AvailableCertificates = GetAvailableCertificatesFromStore();
            return View(model);
        }

        private List<string> CreateDefaultAccounts(string userName, string password)
        {
            var errors = new List<string>();

            try
            {
                UserManagement.CreateRole(Constants.Roles.IdentityServerUsers);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            try
            {
                UserManagement.CreateRole(Constants.Roles.IdentityServerAdministrators);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            if (errors.Count != 0)
            {
                return errors;
            }


            try
            {
                UserManagement.CreateUser(userName, password);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            try
            {
                UserManagement.SetRolesForUser(userName, new string[] { Constants.Roles.IdentityServerAdministrators });
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            return errors;
        }

        #region Helper
        private List<string> GetAvailableCertificatesFromStore()
        {
            var list = new List<string>();
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                foreach (var cert in store.Certificates)
                {
                    // todo: add friendly name
                    list.Add(string.Format("{0}", cert.Subject));
                }
            }
            finally
            {
                store.Close();
            }

            return list;
        }
        #endregion
    }
}
