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
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Helper
{
    /// <summary>
    /// Helper class to retrieve certificates from configuration
    /// </summary>
    public static class X509Certificates
    {
       
        /// <summary>
        /// Retrieves a certificate from the certificate store.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="name">The name.</param>
        /// <param name="findType">Type of the find.</param>
        /// <param name="value">The value.</param>
        /// <returns>A X509Certificate2</returns>
        public static X509Certificate2 GetCertificateFromStore(StoreLocation location, StoreName name, X509FindType findType, object value)
        {
            X509Store store = new X509Store(name, location);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                // work around possible bug in framework
                if (findType == X509FindType.FindByThumbprint)
                {
                    var thumbprint = value.ToString();
                    thumbprint = thumbprint.Trim();
                    thumbprint = thumbprint.Replace(" ", "");

                    foreach (var cert in store.Certificates)
                    {
                        if (string.Equals(cert.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(cert.Thumbprint, thumbprint, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return cert;
                        }
                    }
                }
                if (findType == X509FindType.FindBySerialNumber)
                {
                    var serial = value.ToString();
                    serial = serial.Trim();
                    serial = serial.Replace(" ", "");

                    foreach (var cert in store.Certificates)
                    {
                        if (string.Equals(cert.SerialNumber, serial, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(cert.SerialNumber, serial, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return cert;
                        }
                    }
                }

                var certs = store.Certificates.Find(findType, value, false);

                if (certs.Count != 1)
                {
                    throw new InvalidOperationException(String.Format("Certificate not found: {0}", value));
                }

                return certs[0];
            }
            finally
            {
                store.Close();
            }
        }

        

        /// <summary>
        /// Retrieves a certificate from the local machine / personal certificate store.
        /// </summary>
        /// <param name="subjectDistinguishedName">The subject distinguished name of the certificate.</param>
        /// <returns>A X509Certificate2</returns>
        public static X509Certificate2 GetCertificateFromStore(string subjectDistinguishedName)
        {
            return GetCertificateFromStore(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindBySubjectDistinguishedName,
                subjectDistinguishedName);
        }
    }
}
