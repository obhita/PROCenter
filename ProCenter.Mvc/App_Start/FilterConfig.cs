namespace ProCenter.Mvc.App_Start
{
    #region Using Statements

    using System.Web.Http;
    using System.Web.Mvc;
    using Infrastructure.Filter;
    using Infrastructure.Security;
    using Pillar.Common.InversionOfControl;

    #endregion

    public class FilterConfig
    {
        #region Public Methods and Operators

        public static void RegisterGlobalFilters ( GlobalFilterCollection filters )
        {
            filters.Add ( new ExtendedHandleErrorAttribute () );
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            filters.Add(new RequireHttpsAttribute());
            filters.Add(IoC.CurrentContainer.Resolve<AccessControlSecurityFilter>());
        }

        public static void RegisterWebApiGlobalFilters ( HttpConfiguration config )
        {
            config.Filters.Add(new ExtendedExceptionFilterAttribute());
        }

        #endregion
    }
}