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
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public abstract class CertificateInputModel : IValidatableObject
    {
        public abstract string Name { get; }
        public abstract HttpPostedFileBase File { get; }
        public bool? RemoveCert { get; set; }

        bool certProcessed;
        X509Certificate2 cert;
        public X509Certificate2 Cert
        {
            get
            {
                ProcessCert();
                return cert;
            }
        }

        private void ProcessCert()
        {
            if (certProcessed) return;
            certProcessed = true;

            if (File != null && File.ContentLength > 0)
            {
                using (var ms = new MemoryStream())
                {
                    File.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    var val = new X509Certificate2(bytes);
                    cert = val;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            try
            {
                ProcessCert();
            }
            catch
            {
                errors.Add(new ValidationResult(Resources.CertificateInputModel.ErrorProcessingCertificate, new string[]{Name}));
            }
            return errors;
        }
    }
}