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
namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OIDC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OpenIdConnectConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OpenIdConnectClients",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 128),
                        ClientSecret = c.String(nullable: false),
                        ClientSecretType = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Flow = c.Int(nullable: false),
                        AllowRefreshToken = c.Boolean(nullable: false),
                        AccessTokenLifetime = c.Int(nullable: false),
                        RefreshTokenLifetime = c.Int(nullable: false),
                        RequireConsent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.OpenIdConnectClientsRedirectUris",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RedirectUri = c.String(nullable: false),
                        OpenIdConnectClientEntity_ClientId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OpenIdConnectClients", t => t.OpenIdConnectClientEntity_ClientId)
                .Index(t => t.OpenIdConnectClientEntity_ClientId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.OpenIdConnectClientsRedirectUris", new[] { "OpenIdConnectClientEntity_ClientId" });
            DropForeignKey("dbo.OpenIdConnectClientsRedirectUris", "OpenIdConnectClientEntity_ClientId", "dbo.OpenIdConnectClients");
            DropTable("dbo.OpenIdConnectClientsRedirectUris");
            DropTable("dbo.OpenIdConnectClients");
            DropTable("dbo.OpenIdConnectConfiguration");
        }
    }
}
