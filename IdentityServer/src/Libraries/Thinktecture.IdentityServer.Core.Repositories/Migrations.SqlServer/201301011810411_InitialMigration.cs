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
namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlobalConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false),
                        IssuerUri = c.String(nullable: false),
                        IssuerContactEmail = c.String(nullable: false),
                        DefaultWSTokenType = c.String(nullable: false),
                        DefaultHttpTokenType = c.String(nullable: false),
                        DefaultTokenLifetime = c.Int(nullable: false),
                        MaximumTokenLifetime = c.Int(nullable: false),
                        SsoCookieLifetime = c.Int(nullable: false),
                        RequireEncryption = c.Boolean(nullable: false),
                        RequireRelyingPartyRegistration = c.Boolean(nullable: false),
                        EnableClientCertificateAuthentication = c.Boolean(nullable: false),
                        EnforceUsersGroupMembership = c.Boolean(nullable: false),
                        HttpPort = c.Int(nullable: false),
                        HttpsPort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WSFederationConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        EnableAuthentication = c.Boolean(nullable: false),
                        EnableFederation = c.Boolean(nullable: false),
                        EnableHrd = c.Boolean(nullable: false),
                        AllowReplyTo = c.Boolean(nullable: false),
                        RequireReplyToWithinRealm = c.Boolean(nullable: false),
                        RequireSslForReplyTo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KeyMaterialConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SigningCertificateName = c.String(),
                        DecryptionCertificateName = c.String(),
                        RSASigningKey = c.String(),
                        SymmetricSigningKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WSTrustConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        EnableMessageSecurity = c.Boolean(nullable: false),
                        EnableMixedModeSecurity = c.Boolean(nullable: false),
                        EnableClientCertificateAuthentication = c.Boolean(nullable: false),
                        EnableFederatedAuthentication = c.Boolean(nullable: false),
                        EnableDelegation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FederationMetadataConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OAuth2Configuration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        EnableConsent = c.Boolean(nullable: false),
                        EnableResourceOwnerFlow = c.Boolean(nullable: false),
                        EnableImplicitFlow = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SimpleHttpConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiagnosticsConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnableFederationMessageTracing = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientCertificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Thumbprint = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Delegation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Realm = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RelyingParties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Realm = c.String(nullable: false),
                        TokenLifeTime = c.Int(nullable: false),
                        ReplyTo = c.String(),
                        EncryptingCertificate = c.String(),
                        SymmetricSigningKey = c.String(),
                        ExtraData1 = c.String(),
                        ExtraData2 = c.String(),
                        ExtraData3 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityProvider",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        ShowInHrdSelection = c.Boolean(nullable: false),
                        WSFederationEndpoint = c.String(),
                        IssuerThumbprint = c.String(),
                        ClientID = c.String(),
                        ClientSecret = c.String(),
                        OAuth2ProviderType = c.Int(),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ClientId = c.String(nullable: false),
                        ClientSecret = c.String(nullable: false),
                        RedirectUri = c.String(),
                        NativeClient = c.Boolean(nullable: false),
                        AllowImplicitFlow = c.Boolean(nullable: false),
                        AllowResourceOwnerFlow = c.Boolean(nullable: false),
                        AllowCodeFlow = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Client");
            DropTable("dbo.IdentityProvider");
            DropTable("dbo.RelyingParties");
            DropTable("dbo.Delegation");
            DropTable("dbo.ClientCertificates");
            DropTable("dbo.DiagnosticsConfiguration");
            DropTable("dbo.SimpleHttpConfiguration");
            DropTable("dbo.OAuth2Configuration");
            DropTable("dbo.FederationMetadataConfiguration");
            DropTable("dbo.WSTrustConfiguration");
            DropTable("dbo.KeyMaterialConfiguration");
            DropTable("dbo.WSFederationConfiguration");
            DropTable("dbo.GlobalConfiguration");
        }
    }
}
