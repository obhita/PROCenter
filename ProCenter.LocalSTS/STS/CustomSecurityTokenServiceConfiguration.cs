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
namespace ProCenter.LocalSTS.STS
{
    #region Using Statements

    using System.IdentityModel.Configuration;
    using System.IdentityModel.Tokens;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using System.Web.Configuration;

    #endregion

    /// <summary>
    ///     A custom SecurityTokenServiceConfiguration implementation.
    /// </summary>
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private const string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";
        private static readonly object syncRoot = new object();

        /// <summary>
        ///     CustomSecurityTokenServiceConfiguration constructor.
        /// </summary>
        public CustomSecurityTokenServiceConfiguration()
            : base(WebConfigurationManager.AppSettings[Common.IssuerName],
                   new X509SigningCredentials(CertificateUtil.GetCertificate(
                       StoreName.My, StoreLocation.LocalMachine,
                       WebConfigurationManager.AppSettings[Common.SigningCertificateName])))
        {
            this.SecurityTokenService = typeof (CustomSecurityTokenService);
        }

        /// <summary>
        ///     Provides a model for creating a single Configuration object for the application. The first call creates a new CustomSecruityTokenServiceConfiguration and
        ///     places it into the current HttpApplicationState using the key "CustomSecurityTokenServiceConfigurationKey". Subsequent calls will return the same
        ///     Configuration object.  This maintains any state that is set between calls and improves performance.
        /// </summary>
        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                HttpApplicationState httpAppState = HttpContext.Current.Application;

                var customConfiguration =
                    httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as
                    CustomSecurityTokenServiceConfiguration;

                if (customConfiguration == null)
                {
                    lock (syncRoot)
                    {
                        customConfiguration =
                            httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as
                            CustomSecurityTokenServiceConfiguration;

                        if (customConfiguration == null)
                        {
                            customConfiguration = new CustomSecurityTokenServiceConfiguration();
                            httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration);
                        }
                    }
                }

                return customConfiguration;
            }
        }
    }
}