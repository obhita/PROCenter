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