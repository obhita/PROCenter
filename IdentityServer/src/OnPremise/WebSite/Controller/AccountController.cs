/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class AccountController : AccountControllerBase
    {
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
        public ActionResult SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                if (UserRepository.ValidateUser(model.UserName, model.Password))
                {
                    // establishes a principal, set the session cookie and redirects
                    // you can also pass additional claims to signin, which will be embedded in the session token

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