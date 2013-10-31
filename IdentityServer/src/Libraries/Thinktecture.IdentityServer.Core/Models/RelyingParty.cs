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
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models
{
    public class RelyingParty
    {
        [Required]
        [UIHint("HiddenInput")]
        public string Id { get; set; }
        
        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "Enabled", Description = "EnabledDescription")]
        public bool Enabled { get; set; }
        
        [Required]
        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "Name", Description = "NameDescription")]
        public string Name { get; set; }
        
        [Required]
        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "Realm", Description = "RealmDescription")]
        [AbsoluteUri]
        public Uri Realm { get; set; }

        [UIHint("Enum")]
        [Display(ResourceType = typeof(Resources.Models.RelyingParty), Name = "TokenType", Description = "TokenTypeDescription")]
        public TokenType? TokenType { get; set; }
        
        [Required]
        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "TokenLifeTime", Description = "TokenLifeTimeDescription")]
        public int TokenLifeTime { get; set; }

        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "ReplyTo", Description = "ReplyToDescription")]
        [AbsoluteUri]
        public Uri ReplyTo { get; set; }

        [Display(Order=10002, ResourceType = typeof (Resources.Models.RelyingParty), Name = "EncryptingCertificate", Description = "EncryptingCertificateDescription")]
        public X509Certificate2 EncryptingCertificate { get; set; }

        [Display(Order = 10003, ResourceType = typeof(Resources.Models.RelyingParty), Name = "EncryptingCertificateThumbprint", Description = "EncryptingCertificateThumbprintDescription")]
        public string EncryptingCertificateThumbprint
        {
            get
            {
                if (EncryptingCertificate == null) return null;
                return EncryptingCertificate.Thumbprint;
            }
        }

        [Display(Order = 10001, ResourceType = typeof (Resources.Models.RelyingParty), Name = "SymmetricSigningKey", Description = "SymmetricSigningKeyDescription")]
        public byte[] SymmetricSigningKey { get; set; }

        [Display(ResourceType = typeof (Resources.Models.RelyingParty), Name = "ExtraData1", Description = "ExtraData1Description")]
        public string ExtraData1 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.RelyingParty), Name = "ExtraData2", Description = "ExtraData2Description")]
        public string ExtraData2 { get; set; }

        [Display(ResourceType = typeof(Resources.Models.RelyingParty), Name = "ExtraData3", Description = "ExtraData3Description")]
        public string ExtraData3 { get; set; }
    }
}