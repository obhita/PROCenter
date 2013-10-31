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
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RelyingPartiesViewModel
    {
        IRelyingPartyRepository RelyingPartyRepository { get; set; }

        List<RelyingPartyViewModel> rps = new List<RelyingPartyViewModel>();
        public IEnumerable<RelyingPartyViewModel> RPs
        {
            get
            {
                return rps;
            }
        }

        public RelyingPartiesViewModel(IRelyingPartyRepository relyingPartyRepository)
        {
            RelyingPartyRepository = relyingPartyRepository;
            var query = RelyingPartyRepository.List(-1, -1);
            var items = query.Select(x => new RelyingPartyViewModel { ID = x.Id, DisplayName = x.Name, Enabled = x.Enabled });
            rps.AddRange(items);
        }

        internal void Update(IEnumerable<RelyingPartyViewModel> list)
        {
            foreach (var item in list)
            {
                var dbItem = this.RelyingPartyRepository.Get(item.ID);
                if (dbItem.Enabled != item.Enabled)
                {
                    dbItem.Enabled = item.Enabled;
                    this.RelyingPartyRepository.Update(dbItem);
                }
            }
        }
    }
}