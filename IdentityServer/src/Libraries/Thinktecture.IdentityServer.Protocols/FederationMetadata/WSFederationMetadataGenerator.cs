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
using System.Globalization;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.FederationMetadata
{
    /// <summary>
    /// Handler for dynamic generation of the WS-Federation metadata document
    /// </summary>
    public class WSFederationMetadataGenerator
    {
        Endpoints _endpoints;

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        [Import]
        public IUserRepository UserRepository { get; set; }

        [Import]
        public IClaimsRepository ClaimsRepository { get; set; }

        public WSFederationMetadataGenerator(Endpoints endpoints)
        {
            _endpoints = endpoints;
            Container.Current.SatisfyImportsOnce(this);
        }

        public string Generate()
        {
            var tokenServiceDescriptor = GetTokenServiceDescriptor();
            var id = new EntityId(ConfigurationRepository.Global.IssuerUri);
            var entity = new EntityDescriptor(id);
            entity.SigningCredentials = new X509SigningCredentials(ConfigurationRepository.Keys.SigningCertificate);
            entity.RoleDescriptors.Add(tokenServiceDescriptor);

            var ser = new MetadataSerializer();
            var sb = new StringBuilder(512);

            ser.WriteMetadata(XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true }), entity);
            return sb.ToString();
        }

        private SecurityTokenServiceDescriptor GetTokenServiceDescriptor()
        {
            var tokenService = new SecurityTokenServiceDescriptor();
            tokenService.ServiceDescription = ConfigurationRepository.Global.SiteName;
            tokenService.Keys.Add(GetSigningKeyDescriptor());

            tokenService.PassiveRequestorEndpoints.Add(new EndpointReference(_endpoints.WSFederation.AbsoluteUri));

            tokenService.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml11TokenProfile11));
            tokenService.TokenTypesOffered.Add(new Uri(TokenTypes.OasisWssSaml2TokenProfile11));

            ClaimsRepository.GetSupportedClaimTypes().ToList().ForEach(claimType => tokenService.ClaimTypesOffered.Add(new DisplayClaim(claimType)));
            tokenService.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMessageSecurity)
            {
                var addressMessageUserName = new EndpointAddress(_endpoints.WSTrustMessageUserName, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
                tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMessageUserName.Uri.AbsoluteUri));

                if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
                {
                    var addressMessageCertificate = new EndpointAddress(_endpoints.WSTrustMessageCertificate, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
                    tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMessageCertificate.Uri.AbsoluteUri));
                }
            }
            if (ConfigurationRepository.WSTrust.Enabled && ConfigurationRepository.WSTrust.EnableMixedModeSecurity)
            {
                var addressMixedUserName = new EndpointAddress(_endpoints.WSTrustMixedUserName, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
                tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMixedUserName.Uri.AbsoluteUri));

                if (ConfigurationRepository.WSTrust.EnableClientCertificateAuthentication)
                {
                    var addressMixedCertificate = new EndpointAddress(_endpoints.WSTrustMixedCertificate, null, null, CreateMetadataReader(_endpoints.WSTrustMex), null);
                    tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(addressMixedCertificate.Uri.AbsoluteUri));
                }
            }

            if (tokenService.SecurityTokenServiceEndpoints.Count == 0)
                tokenService.SecurityTokenServiceEndpoints.Add(new EndpointReference(_endpoints.WSFederation.AbsoluteUri));

            return tokenService;
        }

        private KeyDescriptor GetSigningKeyDescriptor()
        {
            var certificate = ConfigurationRepository.Keys.SigningCertificate;

            var clause = new X509SecurityToken(certificate).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>();
            var key = new KeyDescriptor(new SecurityKeyIdentifier(clause));
            key.Use = KeyType.Signing;

            return key;
        }

        private XmlDictionaryReader CreateMetadataReader(Uri mexAddress)
        {
            var metadataSet = new MetadataSet();
            var metadataReference = new MetadataReference(new EndpointAddress(mexAddress), AddressingVersion.WSAddressing10);
            var metadataSection = new MetadataSection(MetadataSection.MetadataExchangeDialect, null, metadataReference);
            metadataSet.MetadataSections.Add(metadataSection);

            var sb = new StringBuilder();
            var w = new StringWriter(sb, CultureInfo.InvariantCulture);
            var writer = XmlWriter.Create(w);

            metadataSet.WriteTo(writer);
            writer.Flush();
            w.Flush();

            var input = new StringReader(sb.ToString());
            var reader = new XmlTextReader(input);
            return XmlDictionaryReader.CreateDictionaryReader(reader);
        }
    }
}