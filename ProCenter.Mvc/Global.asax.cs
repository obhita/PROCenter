﻿#region License Header

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

namespace ProCenter.Mvc
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Services;
    using System.IdentityModel.Tokens;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using StructureMap;

    using ProCenter.Mvc.App_Start;
    using ProCenter.Mvc.Controllers;
    using ProCenter.Mvc.Infrastructure.Binder;
    using ProCenter.Mvc.Infrastructure.Security;
    using ProCenter.Mvc.Infrastructure.Service;
    using ProCenter.Mvc.Infrastructure.Service.Completeness;

    using NLog;
    using NLog.Config;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Infrastructure;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    ///     Main MVC Application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        #region Static Fields

        private static readonly Logger _logger;

        #endregion

        #region Constructors and Destructors

        static MvcApplication ()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition ( "user-context", typeof(UserContextLayoutRenderer) );
            _logger = LogManager.GetCurrentClassLogger ();
        }

        #endregion

        #region Methods

        /// <summary>Application end request.</summary>
        protected void Application_EndRequest ()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects ();
        }

        /// <summary>
        ///     Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Error ( object sender, EventArgs e )
        {
            var exception = Server.GetLastError ();
            if ( exception is SecurityTokenException && exception.Message.StartsWith ( "ID4243:" ) )
            {
                Server.ClearError ();
                var logoutService = IoC.CurrentContainer.Resolve<ILogoutService> ();
                var signoutMessage = logoutService.Logout () as SignOutRequestMessage;
                HttpContext.Current.Response.Redirect ( signoutMessage.WriteQueryString () );
                return;
            }

            // http://www.codeproject.com/Articles/422572/Exception-Handling-in-ASP-NET-MVC
            // http://iamlevi.net/asp-net-mvc-4-ways-handle-exceptions/
            var action = "HttpError";
            if ( exception is HttpException )
            {
                var httpEx = exception as HttpException;

                switch ( httpEx.GetHttpCode () )
                {
                    case (int)HttpStatusCode.NotFound:
                        action = "Http404";
                        break;
                    default:
                        action = "HttpError";
                        break;
                }
            }
            var httpContext = HttpContext.Current;
            _logger.Error ( "{0} {1}", exception.Message, httpContext.Request.Url, exception );

            var currentRouteData = RouteTable.Routes.GetRouteData ( new HttpContextWrapper ( httpContext ) );
            var currentController = " ";
            var currentAction = " ";
            if ( currentRouteData != null )
            {
                if ( currentRouteData.Values["controller"] != null &&
                     !string.IsNullOrEmpty ( currentRouteData.Values["controller"].ToString () ) )
                {
                    currentController = currentRouteData.Values["controller"].ToString ();
                }

                if ( currentRouteData.Values["action"] != null &&
                     !string.IsNullOrEmpty ( currentRouteData.Values["action"].ToString () ) )
                {
                    currentAction = currentRouteData.Values["action"].ToString ();
                }
            }

            httpContext.ClearError ();
            httpContext.Response.Clear ();
            httpContext.Response.StatusCode = exception is HttpException
                ? ( (HttpException)exception ).GetHttpCode ()
                : (int)HttpStatusCode.InternalServerError;
            httpContext.Response.TrySkipIisCustomErrors = true;
            var controller = new ErrorController ();
            var routeData = new RouteData ();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            controller.ViewData.Model = new HandleErrorInfo ( exception, currentController, currentAction );
            ( (IController)controller ).Execute ( new RequestContext ( new HttpContextWrapper ( httpContext ), routeData ) );
        }

        /// <summary>
        ///     Application start.
        /// </summary>
        protected void Application_Start ()
        {
            AreaRegistration.RegisterAllAreas ();
            ProCenterConfig.Bootstrap ();

            ModelBinders.Binders.DefaultBinder = new ProCenterModelBinder ();
            ModelBinders.Binders.Add ( typeof(IEnumerable<LookupDto>), new EnumerableLookupDtoModelBinder () );
            ModelMetadataProviders.Current = IoC.CurrentContainer.Resolve<ResourceModelMetadataProvider> ();
            ModelValidatorProviders.Providers.Add ( new CompletenessModelValidtorProvider () );

            FilterConfig.RegisterGlobalFilters ( GlobalFilters.Filters );
            FilterConfig.RegisterWebApiGlobalFilters ( GlobalConfiguration.Configuration );
            WebApiConfig.Register ( GlobalConfiguration.Configuration );
            RouteConfig.RegisterRoutes ( RouteTable.Routes );
            BundleConfig.RegisterBundles ( BundleTable.Bundles );
        }

        /// <summary>
        ///     Handles the SessionSecurityTokenReceived event of the SessionAuthenticationModule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SessionSecurityTokenReceivedEventArgs" /> instance containing the event data.</param>
        protected void SessionAuthenticationModule_SessionSecurityTokenReceived ( object sender, SessionSecurityTokenReceivedEventArgs e )
        {
            _logger.Debug (
                           string.Format (
                                          "SessionAuthenticationModule_SessionSecurityTokenReceived. Session security token has been read from a cookie. " +
                                          "ValidTo: {0}. ValidFrom: {1} DateTime Utc: {2}",
                               e.SessionToken.ValidTo,
                               e.SessionToken.ValidFrom,
                               DateTime.UtcNow ) );

            // The sliding expiration implementation will extend the token validity interval based on the current interval length when it is detected as expired.
            var utcNow = DateTime.UtcNow;
            var validFrom = e.SessionToken.ValidFrom;
            var validTo = e.SessionToken.ValidTo;

            if ( validTo > utcNow )
            {
                return;
            }

            var sessionAuthenticationModule = sender as SessionAuthenticationModule;

            if ( sessionAuthenticationModule == null )
            {
                return;
            }

            var slidingExpiration = validTo - validFrom;
            var newValidTo = utcNow + slidingExpiration;
            e.SessionToken = sessionAuthenticationModule.CreateSessionSecurityToken (
                                                                                     e.SessionToken.ClaimsPrincipal,
                e.SessionToken.Context,
                utcNow,
                newValidTo,
                e.SessionToken.IsPersistent );
            e.ReissueCookie = true;

            _logger.Debug (
                           string.Format (
                                          "Expired session token detected. " +
                                          "ReissueCookie called to create new session token from SessionAuthenticationModule_SessionSecurityTokenReceived. "
                                          +
                                          "ValidTo: {0}. ValidFrom: {1} DateTime Utc: {2}",
                               e.SessionToken.ValidTo,
                               e.SessionToken.ValidFrom,
                               DateTime.UtcNow ) );
        }

        /// <summary>
        ///     Handles the SessionSecurityTokenCreated event of the WSFederationAuthenticationModule control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SessionSecurityTokenCreatedEventArgs" /> instance containing the event data.</param>
        protected void WSFederationAuthenticationModule_SessionSecurityTokenCreated ( object sender, SessionSecurityTokenCreatedEventArgs e )
        {
#if DEBUG
            e.SessionToken.IsPersistent = false;
#endif
            e.SessionToken.IsReferenceMode = true;
        }

        #endregion
    }
}