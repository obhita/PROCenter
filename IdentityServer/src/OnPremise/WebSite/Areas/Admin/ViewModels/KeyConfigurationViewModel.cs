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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web.Mvc;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    [MetadataType(typeof(KeyMaterialConfiguration))]
    public class KeyConfigurationInputModel : IValidatableObject
    {
        public KeyConfigurationInputModel()
        {
        }
        public KeyConfigurationInputModel(Models.Configuration.KeyMaterialConfiguration keysConfig)
        {
            if (keysConfig.SigningCertificate != null)
            {
                SigningCertificate = keysConfig.SigningCertificate.Subject;
            }
            if (keysConfig.DecryptionCertificate != null)
            {
                DecryptionCertificate = keysConfig.DecryptionCertificate.Subject;
            }

            SymmetricSigningKey = keysConfig.SymmetricSigningKey;
        }

        public string SigningCertificate { get; set; }
        public string DecryptionCertificate { get; set; }
        [UIHint("SymmetricKey")]
        public string SymmetricSigningKey { get; set; }

        public void Update(Repositories.IConfigurationRepository ConfigurationRepository)
        {
            var keys = ConfigurationRepository.Keys;
            try
            {
                var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(SigningCertificate, false).First();

                // make sure we can access the private key
                var pk = cert.PrivateKey;

                keys.SigningCertificate = cert;
            }
            catch (CryptographicException)
            {
                throw new ValidationException(string.Format(Resources.KeyConfigurationViewModel.NoReadAccessToPrivateKey, WindowsIdentity.GetCurrent().Name));
            }

            if (DecryptionCertificate == null)
            {
                keys.DecryptionCertificate = null;
            }
            else
            {
                try
                {
                    var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(DecryptionCertificate, false).First();

                    // make sure we can access the private key
                    var pk = cert.PrivateKey;

                    keys.DecryptionCertificate = cert;
                }
                catch (CryptographicException)
                {
                    throw new ValidationException(string.Format(Resources.KeyConfigurationViewModel.NoReadAccessToPrivateKey, WindowsIdentity.GetCurrent().Name));
                }
            }

            keys.SymmetricSigningKey = SymmetricSigningKey;

            // updates key material config
            ConfigurationRepository.Keys = keys;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            try
            {
                var bytes = Convert.FromBase64String(this.SymmetricSigningKey);
                if (bytes.Length != 32)
                {
                    errors.Add(new ValidationResult(Resources.KeyConfigurationViewModel.InvalidSymmetricKeyLength, new string[] { "SymmetricSigningKey" }));
                }
            }
            catch(FormatException ex)
            {
                errors.Add(new ValidationResult(ex.Message, new string[] { "SymmetricSigningKey" }));
            }
            return errors;
        }
    }

    public class KeyConfigurationViewModel
    {
        public IEnumerable<SelectListItem> AvailableSigningCerts
        {
            get
            {
                var allCerts = GetAvailableCertificatesFromStore().Select(x => new SelectListItem { Text = x }).ToList();
                return allCerts;
            }
        }
        public IEnumerable<SelectListItem> AvailableDecryptionCerts
        {
            get
            {
                var allCerts = GetAvailableCertificatesFromStore().Select(x => new SelectListItem { Text = x }).ToList();
                allCerts.Insert(0, new SelectListItem { Text = Resources.KeyConfigurationViewModel.NoItemSelected, Value = "" });
                return allCerts;
            }
        }

        public KeyConfigurationInputModel Keys { get; set; }
        [Display(ResourceType = typeof (Resources.KeyConfigurationViewModel), Name = "SigningThumbprint")]
        public string SigningCertificateThumbprint { get; set; }

        public KeyConfigurationViewModel(Models.Configuration.KeyMaterialConfiguration config)
        {
            this.Keys = new KeyConfigurationInputModel(config);
            if (config.SigningCertificate != null)
            {
                SigningCertificateThumbprint = config.SigningCertificate.Thumbprint;
            }
        }

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
    }
}