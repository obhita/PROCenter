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