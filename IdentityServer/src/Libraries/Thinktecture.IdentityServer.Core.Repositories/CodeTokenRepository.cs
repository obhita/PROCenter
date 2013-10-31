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
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class CodeTokenRepository : ICodeTokenRepository
    {
        public string AddCode(CodeTokenType type, int clientId, string userName, string scope)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var code = Guid.NewGuid().ToString("N");

                var refreshToken = new CodeToken
                {
                    Type = (int)type,
                    Code = code,
                    ClientId = clientId,
                    Scope = scope,
                    UserName = userName,
                    TimeStamp = DateTime.UtcNow
                };

                entities.CodeTokens.Add(refreshToken);
                entities.SaveChanges();

                return code;
            }
        }

        public bool TryGetCode(string code, out Models.CodeToken token)
        {
            token = null;

            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var entity = (from t in entities.CodeTokens
                              where t.Code.Equals(code, StringComparison.OrdinalIgnoreCase)
                              select t)
                             .FirstOrDefault();

                if (entity != null)
                {
                    token = entity.ToDomainModel();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void DeleteCode(string code)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.CodeTokens.Where(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (item != null)
                {
                    entities.CodeTokens.Remove(item);
                    entities.SaveChanges();
                }
            }
        }

        public IEnumerable<Models.CodeToken> Search(int? clientId, string username, string scope, CodeTokenType type)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var query = 
                    from t in entities.CodeTokens
                    where t.Type == (int)type
                    select t;

                if (clientId != null)
                {
                    query =
                        from t in query
                        where t.ClientId == clientId.Value
                        select t;
                }

                if (!String.IsNullOrWhiteSpace(username))
                {
                    query =
                        from t in query
                        where t.UserName.Contains(username)
                        select t;
                }

                if (!String.IsNullOrWhiteSpace(scope))
                {
                    query =
                        from t in query
                        where t.Scope.Contains(scope)
                        select t;
                }

                var results = query.ToArray().Select(x => x.ToDomainModel());
                return results;
            }
        }
    }
}
