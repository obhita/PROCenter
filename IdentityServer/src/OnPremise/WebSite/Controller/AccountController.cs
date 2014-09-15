/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web.Security;
    using App_LocalResources.Account;
    using Controller.Api;
    using NLog;
    using Resources;
    using SetupAccountModel = ViewModels.SetupAccountModel;
    using SignInModel = ViewModels.SignInModel;

    public class AccountController : AccountControllerBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AccountController() : base()
        { }

        public AccountController(IUserRepository userRepository, IConfigurationRepository configurationRepository) : base(userRepository, configurationRepository)
        { }
        
        // shows the signin screen
        public ActionResult SignIn(string returnUrl, bool mobile=false)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party

            var vm = new SignInModel()
            {
                ReturnUrl = returnUrl,
                ShowClientCertificateLink = ConfigurationRepository.Global.EnableClientCertificateAuthentication
            };

            if (mobile) vm.IsSigninRequest = true;
            return View(vm);
        }

        // handles the signin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var setupNeeded = false;
                var user = Membership.GetUser(model.UserName);
                if ( user != null && user.LastPasswordChangedDate > user.LastLoginDate )
                {
                    setupNeeded = true;
                }
                if (UserRepository.ValidateUser(model.UserName, model.Password))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token
                    if ( user.PasswordQuestion == null || setupNeeded )
                    {
                        model.Password = "change";
                        return RedirectToAction ( "SetupAccount", model );
                    }

                    return SignIn(
                        model.UserName, 
                        AuthenticationMethods.Password, 
                        model.ReturnUrl, 
                        model.EnableSSO, 
                        ConfigurationRepository.Global.SsoCookieLifetime);
                }
            }

            ModelState.AddModelError("", Resources.AccountController.IncorrectCredentialsNoAuthorization);

            model.ShowClientCertificateLink = ConfigurationRepository.Global.EnableClientCertificateAuthentication;
            return View(model);
        }

        // handles client certificate based signin
        public ActionResult CertificateSignIn(string returnUrl)
        {
            if (!ConfigurationRepository.Global.EnableClientCertificateAuthentication)
            {
                return new HttpNotFoundResult();
            }

            var clientCert = HttpContext.Request.ClientCertificate;

            if (clientCert != null && clientCert.IsPresent && clientCert.IsValid)
            {
                string userName;
                if (UserRepository.ValidateUser(new X509Certificate2(clientCert.Certificate), out userName))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token

                    return SignIn(
                        userName, 
                        AuthenticationMethods.X509, 
                        returnUrl, 
                        false, 
                        ConfigurationRepository.Global.SsoCookieLifetime);
                }
            }

            return View("Error");
        }

        // shows the signin screen
        public ActionResult SetupAccount(SignInModel model)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party

            var vm = new SetupAccountModel(model);

            vm.Password = null;

            SetupSecurityQuestions ();

            return View(vm);
        }

        // handles the signin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult SetupAccount(SetupAccountModel model)
        {
            if (ModelState.IsValid)
            {
                if (UserRepository.ValidateUser(model.UserName, model.Password))
                {
                    var user = Membership.GetUser(model.UserName);
                    if ( user.ChangePasswordQuestionAndAnswer ( model.Password, model.SecurityQuestion, model.SecurityAnswer ) )
                    {
                        user.ChangePassword ( model.Password, model.NewPassword );
                        UserRepository.ValidateUser ( model.UserName, model.NewPassword );
                        return SignIn ( model.UserName,
                            AuthenticationMethods.X509,
                            model.ReturnUrl,
                            false,
                            ConfigurationRepository.Global.SsoCookieLifetime );
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.AccountController.BadSecurityAnswer);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.AccountController.IncorrectCredentialsNoAuthorization);
                }
            }
            SetupSecurityQuestions ();
            return View(model);
        }

        private void SetupSecurityQuestions ()
        {
            var questions = new List<string>();
            var index = 1;
            var key = "Question";
            var value = SecurityQuestions.ResourceManager.GetString(key);
            while (value != null)
            {
                questions.Add(value);
                value = SecurityQuestions.ResourceManager.GetString(key + index);
                index++;
            }

            ViewData["SecurityQuestions"] = questions.Select(q => new SelectListItem { Text = q });
        }

        public ActionResult ForgotPassword(string returnUrl)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party

            var vm = new ForgotPasswordModel()
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            // you can call AuthenticationHelper.GetRelyingPartyDetailsFromReturnUrl to get more information about the requested relying party
            if ( forgotPasswordModel.SecurityAnswer == null )
            {
                var user = Membership.GetUser ( forgotPasswordModel.UserName );
                if ( user == null )
                {
                    _logger.Debug("Unknown user: " + forgotPasswordModel.UserName);
                    ModelState.AddModelError ( "forgotPasswordModel", ForgotPassword_cshtml.UnknownUser );
                }
                else
                {
                    forgotPasswordModel.SecurityQuestion = user.PasswordQuestion;
                    if ( forgotPasswordModel.SecurityQuestion == null || user.Email == null )
                    {
                        ModelState.AddModelError("forgotPasswordModel", ForgotPassword_cshtml.CannotReset);
                    }
                }
            }
            else
            {
                var user = Membership.GetUser(forgotPasswordModel.UserName);
                if (user == null)
                {
                    _logger.Debug("Unknown user: " + forgotPasswordModel.UserName);
                    ModelState.AddModelError("forgotPasswordModel", ForgotPassword_cshtml.UnknownUser);
                }
                else
                {
                    try
                    {
                        var password = user.ResetPassword ( forgotPasswordModel.SecurityAnswer );
                        MembershipController.SendEmailNotification ( user, password );
                        ViewData["PasswordResetSent"] = true;
                        _logger.Trace ( "Password reset for user: " + forgotPasswordModel.UserName );
                    }
                    catch ( MembershipPasswordException exception )
                    {
                        _logger.DebugException("Error resetting password for user: " + user.Email, exception);
                        ModelState.AddModelError("forgotPasswordModel", ForgotPassword_cshtml.InvalidResponse);
                    }
                    catch ( Exception exception)
                    {
                        _logger.DebugException ( "Error resetting password for user: " + user.Email, exception );
                        ModelState.AddModelError("forgotPasswordModel", ForgotPassword_cshtml.UnknownError);
                    }
                }
            }
            return View(forgotPasswordModel);
        }

        // handle external EHR authentication
        public ActionResult ExternalSignIn()
        {
            NameValueCollection form = Request.Form;
            if (form != null && form.Keys.OfType<string>().Contains("Token"))
            {
                var signature = Convert.FromBase64String(form["Token"]);
                var noTokenForm = new NameValueCollection(form);
                noTokenForm.Remove("Token");
                var contentString = FormEncode(noTokenForm);
                var hash = SHA1.Create().ComputeHash(Encoding.Unicode.GetBytes(contentString));

                var ehrId = form["EhrId"];
                var signingCertName = ehrId + "Cert";
                var key = GetPublicKey(signingCertName);

                var deformatter = new RSAPKCS1SignatureDeformatter(key);
                deformatter.SetHashAlgorithm("SHA1");
                var verify = deformatter.VerifySignature(hash, signature);
                if (verify)
                {
                    string userId = form["UserId"];
                    string returnUrl = form["ReturnUrl"];
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token
                    return SignIn( // todo: one login for each EHR system, login using EHR id, and overwrite outputClaimIdentity Name with userId. 
                        userId, //Note: only work if maps to a login name
                        AuthenticationMethods.Password,
                        returnUrl,
                        true, // isPersistent
                        ConfigurationRepository.Global.SsoCookieLifetime,
                        new List<System.Security.Claims.Claim>
                            {
                                new System.Security.Claims.Claim("EhrUserName", ehrId)
                            });
                }
            }

            return View("Error");
        }

        private static AsymmetricAlgorithm GetPublicKey(string signingCertName)
        {
            var my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            my.Open(OpenFlags.ReadOnly);

            // Look for the certificate with specific subject 
            var publicKey = my.Certificates.Cast<X509Certificate2>()
                .Where(cert => cert.Subject.Contains("CN=" + signingCertName))
                .Select(cert => cert.PublicKey)
                .FirstOrDefault();
            if (publicKey == null)
            {
                throw new Exception("Valid certificate was not found");
            }

            return publicKey.Key;
        }

        private static string FormEncode(NameValueCollection nameValueCollection)
        {
            return string.Join("&", nameValueCollection.Keys.OfType<string>().Select(key => string.Format("{0}={1}", key, nameValueCollection[key])));
        }
    }
}