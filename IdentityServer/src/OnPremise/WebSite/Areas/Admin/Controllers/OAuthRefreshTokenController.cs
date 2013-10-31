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
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class OAuthRefreshTokenController : System.Web.Mvc.Controller
    {
        [Import]
        public IClientsRepository clientRepository { get; set; }
        [Import]
        ICodeTokenRepository codeTokenRepository { get; set; }

        public OAuthRefreshTokenController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OAuthRefreshTokenController(
            IClientsRepository clientRepository, ICodeTokenRepository codeTokenRepository)
        {
            this.clientRepository = clientRepository;
            this.codeTokenRepository = codeTokenRepository;
        }

        public ActionResult Index(TokenSearchCriteria searchCriteria)
        {
            var vm = new OAuthRefreshTokenIndexViewModel(searchCriteria, clientRepository, codeTokenRepository);
            return View("Index", vm);
        }
        
        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(TokenSearchCriteria searchCriteria)
        {
            var vm = new OAuthRefreshTokenIndexViewModel(searchCriteria, clientRepository, codeTokenRepository, true);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteToken(string token, TokenSearchCriteria searchCriteria)
        {
            codeTokenRepository.DeleteCode(token);
            return RedirectToAction("Index", new { searchCriteria.Username, searchCriteria.Scope, searchCriteria.ClientID });
        }
    }
}
