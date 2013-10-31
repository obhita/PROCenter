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
namespace ProCenter.Service.Handler.Security
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Common;
    using Domain.SecurityModule;
    using Service.Message.Security;

    #endregion

    public class ChangePasswordRequestHandler : ServiceRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public ChangePasswordRequestHandler(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        protected override void Handle(ChangePasswordRequest request, ChangePasswordResponse response)
        {
            var systemAccount = _systemAccountRepository.GetByKey(request.SystemAccountKey);
            if (systemAccount != null)
            {
                using (var httpClient = new HttpClient {BaseAddress = new Uri(request.BaseBaseIdentityServerUri)})
                {
                    httpClient.SetToken("Session", request.Token);
                    var httpResponseMessage = httpClient.GetAsync("api/membership/GetUserByEmail?email=" + systemAccount.Identifier).Result;
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        var membershipUserDtos = httpResponseMessage.Content.ReadAsAsync<IEnumerable<MembershipUserDto>>().Result.ToList();

                        if (membershipUserDtos.Count == 0)
                        {
                            response.ResultCode = "NoAccount";
                            return;
                        }
                        if (membershipUserDtos.Count != 1)
                        {
                            response.ResultCode = "MultipleAccount";
                            return;
                        }

                        var membershipUserDto = membershipUserDtos[0];

                        var path = "api/membership/ChangePassword/" + membershipUserDto.Username + "?oldPassword=" + request.OldPassword + "&newPassword=" + request.NewPassword;
                        httpResponseMessage = httpClient.GetAsync(path).Result;
                        response.ResultCode = httpResponseMessage.StatusCode == HttpStatusCode.OK ? "Succeed" : "InvalidCredentials";
                    }
                    else
                    {
                        //var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        response.ResultCode = "InvalidCredentials";
                    }
                }
            }
        }
    }
}