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
namespace ProCenter.LocalSTS
{
    #region Using Statements

    using System;
    using System.Globalization;
    using System.IdentityModel;
    using System.IdentityModel.Services;
    using System.Security.Claims;
    using System.Threading;
    using System.Web.UI;
    using STS;

    #endregion

    public partial class Default : Page
    {
        /// <summary>
        ///     Performs WS-Federation Passive Protocol processing.
        /// </summary>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string action = Request.QueryString[WSFederationConstants.Parameters.Action];

            try
            {
                if (action == WSFederationConstants.Actions.SignIn)
                {
                    // Process signin request.
                    var requestMessage = (SignInRequestMessage) WSFederationMessage.CreateFromUri(Request.Url);
                    if (User != null && User.Identity.IsAuthenticated)
                    {
                        SecurityTokenService sts =
                            new CustomSecurityTokenService(CustomSecurityTokenServiceConfiguration.Current);
                        SignInResponseMessage responseMessage =
                            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User as ClaimsPrincipal, sts);
                        FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, Response);
                    }
                    else
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
                else if (action == WSFederationConstants.Actions.SignOut)
                {
                    // Process signout request.
                    var requestMessage = (SignOutRequestMessage) WSFederationMessage.CreateFromUri(Request.Url);
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, User as ClaimsPrincipal, requestMessage.Reply, Response);
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.InvariantCulture,
                                      "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.",
                                      String.IsNullOrEmpty(action) ? "<EMPTY>" : action,
                                      WSFederationConstants.Parameters.Action,
                                      WSFederationConstants.Actions.SignIn,
                                      WSFederationConstants.Actions.SignOut));
                }
            }
            catch (ThreadAbortException)
            {
                // Swallow exception 
            }
            catch (Exception genericException)
            {
                throw new Exception(
                    "An unexpected error occurred when processing the request. See inner exception for details.",
                    genericException);
            }
        }
    }
}