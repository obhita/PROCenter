#region Using Statements

using System.Web.Http;

#endregion

namespace ProCenter.Mvc.App_Start
{
    using System.Web.Routing;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApiGet",
                                "api/{controller}/{action}/{key}",
                                new { key = RouteParameter.Optional, action = "Get" },
                                new { httpMethod = new HttpMethodConstraint("GET"), key = new GuidConstraint() }
                );

            config.Routes.MapHttpRoute("DefaultApiPost",
                                "api/{controller}/{action}/{key}",
                                new { key = RouteParameter.Optional, action = "Post" },
                                new { httpMethod = new HttpMethodConstraint("POST"), key = new GuidConstraint() }
                );

            config.Routes.MapHttpRoute("DefaultApiPut",
                                "api/{controller}/{action}/{key}",
                                new { key = RouteParameter.Optional, action = "Put" },
                                new { httpMethod = new HttpMethodConstraint("PUT"), key = new GuidConstraint() }
                );
        }
    }
}