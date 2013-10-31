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
    using App_Start;
    using Controllers;
    using Domain.SecurityModule;
    using Infrastructure.Binder;
    using Infrastructure.Security;
    using Infrastructure.Service;
    using Infrastructure.Service.Completeness;
    using NLog;
    using Pillar.Common.InversionOfControl;
    using Service.Message.Common.Lookups;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Main MVC Application
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region Methods

        /// <summary>
        /// Application start.
        /// </summary>
        protected void Application_Start ()
        {
            AreaRegistration.RegisterAllAreas ();
            ProCenterConfig.Bootstrap ();

            ModelBinders.Binders.DefaultBinder = new ProCenterModelBinder();
            ModelBinders.Binders.Add(typeof(IEnumerable<LookupDto>), new EnumerableLookupDtoModelBinder());
            ModelMetadataProviders.Current = IoC.CurrentContainer.Resolve<ResourceModelMetadataProvider>();
            ModelValidatorProviders.Providers.Add(new CompletenessModelValidtorProvider());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterWebApiGlobalFilters(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes ( RouteTable.Routes );
            BundleConfig.RegisterBundles ( BundleTable.Bundles );
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is SecurityTokenException && exception.Message.StartsWith("ID4243:"))
            {
                Server.ClearError();
                var logoutService = IoC.CurrentContainer.Resolve<ILogoutService>();
                var signoutMessage = logoutService.Logout() as SignOutRequestMessage;
                HttpContext.Current.Response.Redirect(signoutMessage.WriteQueryString());
                return;
            }

            // http://www.codeproject.com/Articles/422572/Exception-Handling-in-ASP-NET-MVC
            // http://iamlevi.net/asp-net-mvc-4-ways-handle-exceptions/
            var action = "HttpError";
            if (exception is HttpException)
            {
                var httpEx = exception as HttpException;

                switch (httpEx.GetHttpCode())
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
            Logger.Error("{0} {1}", exception.Message, httpContext.Request.Url, exception);

            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            // currentController = Request.RequestContext.RouteData.Values["controller"].ToString();
            var currentAction = " ";
            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = exception is HttpException
                                                  ? ((HttpException)exception).GetHttpCode()
                                                  : (int)HttpStatusCode.InternalServerError;
            httpContext.Response.TrySkipIisCustomErrors = true;
            var controller = new ErrorController();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }

        protected void WSFederationAuthenticationModule_SessionSecurityTokenCreated ( object sender, SessionSecurityTokenCreatedEventArgs e )
        {
#if DEBUG
            e.SessionToken.IsPersistent = false;
#endif
            e.SessionToken.IsReferenceMode = true;
        }

        protected void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            Logger.Debug(string.Format("SessionAuthenticationModule_SessionSecurityTokenReceived. Session security token has been read from a cookie. ValidTo: {0}. ValidFrom: {1} DateTime Utc: {2}", e.SessionToken.ValidTo, e.SessionToken.ValidFrom, DateTime.UtcNow));

            // The sliding expiration implementation will extend the token validity interval based on the current interval length when it is detected as expired.
            var utcNow = DateTime.UtcNow;
            var validFrom = e.SessionToken.ValidFrom;
            var validTo = e.SessionToken.ValidTo;

            if (validTo > utcNow)
            {
                return;
            }

            var sessionAuthenticationModule = sender as SessionAuthenticationModule;

            if (sessionAuthenticationModule == null)
            {
                return;
            }

            var slidingExpiration = validTo - validFrom;
            var newValidTo = utcNow + slidingExpiration;
            e.SessionToken = sessionAuthenticationModule.CreateSessionSecurityToken(
                e.SessionToken.ClaimsPrincipal, e.SessionToken.Context, utcNow, newValidTo, e.SessionToken.IsPersistent);
            e.ReissueCookie = true;

            Logger.Debug(string.Format("Expired session token detected. ReissueCookie called to create new session token from SessionAuthenticationModule_SessionSecurityTokenReceived. ValidTo: {0}. ValidFrom: {1} DateTime Utc: {2}", e.SessionToken.ValidTo, e.SessionToken.ValidFrom, DateTime.UtcNow));
        }

        #endregion
    }
}