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
using System.Linq;
using Entities = Thinktecture.IdentityServer.Repositories.Sql;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public virtual bool SupportsWriteAccess
        {
            get { return true; }
        }

        public virtual Models.Configuration.GlobalConfiguration Global
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.GlobalConfiguration.First<Entities.Configuration.GlobalConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.GlobalConfiguration.First<Entities.Configuration.GlobalConfiguration>();
                    entities.GlobalConfiguration.Remove(entity);

                    entities.GlobalConfiguration.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.DiagnosticsConfiguration Diagnostics
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Diagnostics.First<Entities.Configuration.DiagnosticsConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Diagnostics.First<Entities.Configuration.DiagnosticsConfiguration>();
                    entities.Diagnostics.Remove(entity);

                    entities.Diagnostics.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.KeyMaterialConfiguration Keys
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Keys.FirstOrDefault<Entities.Configuration.KeyMaterialConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Keys.FirstOrDefault<Entities.Configuration.KeyMaterialConfiguration>();

                    if (entity != null)
                    {
                        entities.Keys.Remove(entity);
                    }

                    entities.Keys.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.WSFederationConfiguration WSFederation
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSFederation.First<Entities.Configuration.WSFederationConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSFederation.First<Entities.Configuration.WSFederationConfiguration>();
                    entities.WSFederation.Remove(entity);

                    entities.WSFederation.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.FederationMetadataConfiguration FederationMetadata
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.FederationMetadata.First<Entities.Configuration.FederationMetadataConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.FederationMetadata.First<Entities.Configuration.FederationMetadataConfiguration>();
                    entities.FederationMetadata.Remove(entity);

                    entities.FederationMetadata.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.WSTrustConfiguration WSTrust
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSTrust.First<Entities.Configuration.WSTrustConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSTrust.First<Entities.Configuration.WSTrustConfiguration>();
                    entities.WSTrust.Remove(entity);

                    entities.WSTrust.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.OAuth2Configuration OAuth2
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.OAuth2.First<Entities.Configuration.OAuth2Configuration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.OAuth2.First<Entities.Configuration.OAuth2Configuration>();
                    entities.OAuth2.Remove(entity);

                    entities.OAuth2.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public virtual Models.Configuration.SimpleHttpConfiguration SimpleHttp
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.SimpleHttp.First<Entities.Configuration.SimpleHttpConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.SimpleHttp.First<Entities.Configuration.SimpleHttpConfiguration>();
                    entities.SimpleHttp.Remove(entity);

                    entities.SimpleHttp.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        // todo: wire up with DB
        public Models.Configuration.AdfsIntegrationConfiguration AdfsIntegration
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.AdfsIntegration.First<Entities.Configuration.AdfsIntegrationConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.AdfsIntegration.First<Entities.Configuration.AdfsIntegrationConfiguration>();
                    entities.AdfsIntegration.Remove(entity);

                    entities.AdfsIntegration.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }


        public Models.Configuration.OpenIdConnectConfiguration OpenIdConnect
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.OpenIdConnect.First<Entities.Configuration.OpenIdConnectConfiguration>();
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.OpenIdConnect.First<Entities.Configuration.OpenIdConnectConfiguration>();
                    entities.OpenIdConnect.Remove(entity);

                    entities.OpenIdConnect.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }
    }
}
