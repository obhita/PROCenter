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
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using Thinktecture.IdentityServer.Configuration;

namespace Thinktecture.IdentityServer.Repositories
{
    public class RepositoryExportProvider : ExportProvider
    {
        private Dictionary<string, string> _mappings;

        public RepositoryExportProvider()
        {
            var section = ConfigurationManager.GetSection(RepositoryConfigurationSection.SectionName) as RepositoryConfigurationSection;

            _mappings = new Dictionary<string, string>
            {
                { typeof(IConfigurationRepository).FullName, section.TokenServiceConfiguration },
                { typeof(IUserRepository).FullName, section.UserValidation },
                { typeof(IUserManagementRepository).FullName, section.UserManagement },
                { typeof(IClaimsRepository).FullName, section.ClaimsRepository },
                { typeof(IRelyingPartyRepository).FullName, section.RelyingParties },
                { typeof(IClientCertificatesRepository).FullName, section.ClientCertificates},
                { typeof(IDelegationRepository).FullName, section.Delegation},
                { typeof(ICacheRepository).FullName, section.Caching },
                { typeof(IIdentityProviderRepository).FullName, section.IdentityProvider },
                { typeof(IClaimsTransformationRulesRepository).FullName, section.ClaimsTransformationRules },
                { typeof(IClientsRepository).FullName, section.ClientsRepository },
                { typeof(ICodeTokenRepository).FullName, section.CodeTokenRepository },
                { typeof(IOpenIdConnectClientsRepository).FullName, section.OpenIdConnectClientsRepository },
                { typeof(IStoredGrantRepository).FullName, section.StoredGrantRepository }
            };
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var exports = new List<Export>();

            string implementingType;
            if (_mappings.TryGetValue(definition.ContractName, out implementingType))
            {
                var t = Type.GetType(implementingType);
                if (t == null)
                {
                    throw new InvalidOperationException("Type not found for interface: " + definition.ContractName);
                }

                var instance = t.GetConstructor(Type.EmptyTypes).Invoke(null);
                var exportDefintion = new ExportDefinition(definition.ContractName, new Dictionary<string, object>());
                var toAdd = new Export(exportDefintion, () => instance);

                exports.Add(toAdd);
            }

            return exports;
        }
    }
}