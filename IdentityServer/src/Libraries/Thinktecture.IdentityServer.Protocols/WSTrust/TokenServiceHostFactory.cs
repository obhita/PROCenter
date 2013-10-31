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
using System.ComponentModel.Composition;
using System.IdentityModel.Configuration;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Thinktecture.IdentityModel.WSTrust;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.WSTrust
{
    /// <summary>
    /// Abstracts away the details of the WS-Trust ServiceHost creation and configuration
    /// </summary>
    public class TokenServiceHostFactory : ServiceHostFactory
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public TokenServiceHostFactory() : base()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        /// <summary>
        /// Creates a service host to process WS-Trust 1.3 requests
        /// </summary>
        /// <param name="constructorString">The constructor string.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        /// <returns>A WS-Trust ServiceHost</returns>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var globalConfiguration = ConfigurationRepository.Global;
            var config = CreateSecurityTokenServiceConfiguration(constructorString);
            var host = new WSTrustServiceHost(config, baseAddresses);
            
            // add behavior for load balancing support
            host.Description.Behaviors.Add(new UseRequestHeadersForMetadataAddressBehavior());

            // modify address filter mode for load balancing
            var serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.AddressFilterMode = AddressFilterMode.Any;

            // add and configure a mixed mode security endpoint
            if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMixedModeSecurity)
            {
                EndpointIdentity epi = null;
                
                if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
                {
                    var sep2 = host.AddServiceEndpoint(
                        typeof(IWSTrust13SyncContract),
                        new CertificateWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                        Endpoints.Paths.WSTrustMixedCertificate);

                    if (epi != null)
                    {
                        sep2.Address = new EndpointAddress(sep2.Address.Uri, epi);
                    }
                }

                var sep = host.AddServiceEndpoint(
                    typeof(IWSTrust13SyncContract),
                    new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                    Endpoints.Paths.WSTrustMixedUserName);

                if (epi != null)
                {
                    sep.Address = new EndpointAddress(sep.Address.Uri, epi);
                }
            }

            // add and configure a message security endpoint
            if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMessageSecurity)
            {
                var credential = new ServiceCredentials();
                credential.ServiceCertificate.Certificate = ConfigurationRepository.Keys.SigningCertificate;
                host.Description.Behaviors.Add(credential);

                if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
                {
                    host.AddServiceEndpoint(
                        typeof(IWSTrust13SyncContract),
                        new CertificateWSTrustBinding(SecurityMode.Message),
                        Endpoints.Paths.WSTrustMessageCertificate);
                }

                host.AddServiceEndpoint(
                    typeof(IWSTrust13SyncContract),
                    new UserNameWSTrustBinding(SecurityMode.Message),
                    Endpoints.Paths.WSTrustMessageUserName);
            }

            return host;
        }

        protected virtual SecurityTokenServiceConfiguration CreateSecurityTokenServiceConfiguration(string constructorString)
        {
            Type type = Type.GetType(constructorString, true);
            if (!type.IsSubclassOf(typeof(SecurityTokenServiceConfiguration)))
            {
                throw new InvalidOperationException("SecurityTokenServiceConfiguration");
            }

            return (Activator.CreateInstance(
                type, 
                BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, 
                null, 
                null, 
                null) as SecurityTokenServiceConfiguration);
        }
    }
}