namespace Thinktecture.IdentityServer.Web.Controller.Api
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Text;
    using System.Threading;
    using System.Web.Configuration;
    using System.Web.Http;
    using System.Web.Security;
    using IdentityModel.Authorization.WebApi;
    using ViewModels;

    #endregion

    [ClaimsAuthorize(Constants.Actions.WebApi, Constants.Resources.General)] 
    public class MembershipController : ApiController
    {
        public HttpResponseMessage Get(string username)
        {
            try
            {
                var user = Membership.GetUser(username);
                if (user == null)
                {
                    var httpError = new HttpError(string.Format("The username '{0}' does not exist.", username));
                    httpError["error_sub_code"] = 1003;
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
                }
                return Request.CreateResponse(HttpStatusCode.OK, Map(user));
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot retrieve user by username '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1010;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }
        }

        public HttpResponseMessage GetUserByEmail(string email)
        {
            try
            {
                var users = Membership.FindUsersByEmail(email);
                return Request.CreateResponse(HttpStatusCode.OK, Map(users));
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot retrieve user by email '{0}'.", email);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1009;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }
        }

        [AcceptVerbs("GET")]
        public HttpResponseMessage Create(string username, string email)
        {
            // Error handling : http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling
            // Web Api return HttpResponseMessage http://stackoverflow.com/questions/12264088/asp-net-web-api-return-clr-object-or-httpresponsemessage
            MembershipUser user;
            try
            {
                user = Membership.GetUser(username);
                if (user != null)
                {
                    //var response = new HttpResponseMessage(HttpStatusCode.Conflict)
                    //    {
                    //        Content = new StringContent(string.Format("The username '{0}' is already in use.", username)),
                    //        ReasonPhrase = "The username is already in use."
                    //    };
                    //throw new HttpResponseException(response);

                    var message = string.Format("The username '{0}' is already in use.", username);
                    var httpError = new HttpError(message);
                    httpError["error_sub_code"] = 1001; //can add custom Key-Values to HttpError
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot retrieve user by username '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1010;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }

            string password;
            try
            {
                password = Membership.GeneratePassword(10, 3);
                user = Membership.CreateUser(username, password, email);
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot create user '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1005;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }

            try
            {
                SetRolesForUser(username, new[] {Constants.Roles.IdentityServerUsers});
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot set role for user '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1007;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }

            try
            {
                SendEmailNotification(user, password);
                return Request.CreateResponse(HttpStatusCode.OK, Map(user));
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot send email out for '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1006;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }
        }

        [AcceptVerbs("GET")]
        public HttpResponseMessage Unlock(string username)
        {
            var user = Membership.GetUser(username);
            if (user == null)
            {
                var httpError = new HttpError(string.Format("The username '{0}' does not exist.", username));
                httpError["error_sub_code"] = 1003;
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
            }
            if (!user.IsLockedOut)
            {
                var httpError = new HttpError("The user is not locked out.");
                httpError["error_sub_code"] = 1004;
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, httpError);
            }
            user.UnlockUser();
            return Request.CreateResponse(HttpStatusCode.OK, Map(user));
        }

        [AcceptVerbs("GET")]
        public HttpResponseMessage ChangePassword(string username, string oldPassword, string newPassword)
        {
            var validUser = Membership.ValidateUser(username, oldPassword);
            if (!validUser)
            {
                var httpError = new HttpError("Invalid username/password.");
                httpError["error_sub_code"] = 1002;
                return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, httpError);
            }
            try
            {
                var user = Membership.GetUser(username);
                user.ChangePassword(oldPassword, newPassword);
                return Request.CreateResponse(HttpStatusCode.OK, Map(user));
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot change password for user '{0}'.", username);
                var httpError = new HttpError(message);
                httpError["error_sub_code"] = 1008;
                httpError["error"] = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);
            }
        }

        private static void SendEmailNotification(MembershipUser user, string password)
        {
            var fullname = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(user.UserName.Replace('.', ' ').ToLower());
            var body = string.Format(EmailTemplate.PasswordSetupMessage, fullname, user.UserName.ToLower(), password);
            using (var message = new MailMessage
                {
                    Subject = WebConfigurationManager.AppSettings["EmailWelcomeSubject"],
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                })
            {
                message.To.Add(new MailAddress(user.Email));
                var cc = WebConfigurationManager.AppSettings["EmailCC"];
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    message.CC.Add(new MailAddress(cc));
                }

                var smtp = new SmtpClient();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                smtp.Send(message);
            }
        }

        private static void SetRolesForUser(string userName, IEnumerable<string> roles)
        {
            var userRoles = Roles.GetRolesForUser(userName);

            if (userRoles.Length != 0)
            {
                Roles.RemoveUserFromRoles(userName, userRoles);
            }

            if (roles.Any())
            {
                Roles.AddUserToRoles(userName, roles.ToArray());
            }
        }

        private static MembershipUserDto Map(MembershipUser user)
        {
            return new MembershipUserDto
                {
                    Username = user.UserName,
                    NameIdentifier = user.Email,
                    Email = user.Email,
                    IsApproved = user.IsApproved,
                    IsLockedOut = user.IsLockedOut,
                    LastLockoutDate = user.LastLockoutDate
                };
        }

        private static IEnumerable<MembershipUserDto> Map(MembershipUserCollection users)
        {
            return users.Cast<MembershipUser>().Select(Map).ToList();
        }
    }
}