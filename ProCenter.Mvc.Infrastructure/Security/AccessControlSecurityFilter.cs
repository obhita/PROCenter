namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Web;
    using System.Web.Mvc;
    using Pillar.Security.AccessControl;

    #endregion

    public class AccessControlSecurityFilter : AuthorizeAttribute
    {
        #region Constants

        private const string Label = "PROCenter.Mvc.ClaimsAuthorizeAttribute";

        #endregion

        #region Fields

        private readonly IAccessControlManager _accessControlManager;

        #endregion

        #region Constructors and Destructors

        public AccessControlSecurityFilter ( IAccessControlManager accessControlManager )
        {
            _accessControlManager = accessControlManager;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnAuthorization ( AuthorizationContext filterContext )
        {
            filterContext.HttpContext.Items[Label] = filterContext;
            base.OnAuthorization ( filterContext );
        }

        #endregion

        #region Methods

        protected override bool AuthorizeCore ( HttpContextBase httpContext )
        {
            return CheckAccess ( httpContext.Items[Label] as AuthorizationContext );
        }

        protected virtual bool CheckAccess ( AuthorizationContext filterContext )
        {
            var resourceRequest = new ResourceRequest
                {
                   filterContext.Controller.GetType ().FullName,
                               filterContext.ActionDescriptor.ActionName,
                               filterContext.HttpContext.Request.HttpMethod
                };
            return _accessControlManager.CanAccess ( resourceRequest );
        }

        #endregion
    }
}