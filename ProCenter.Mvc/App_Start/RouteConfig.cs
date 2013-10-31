#region Using Statements



#endregion

namespace ProCenter.Mvc.App_Start
{
    #region

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{key}",
                defaults: new {controller = "Home", action = "Index", key = UrlParameter.Optional}
                );
        }
    }
}