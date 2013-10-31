namespace ProCenter.Mvc.App_Start
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Routing;

    #endregion

    public class GuidConstraint : IHttpRouteConstraint
    {
        // http://stackoverflow.com/questions/15681330/webapi-controller-with-two-get-actions
        // https://github.com/stevenbey/elfar/blob/master/Elfar.WebApi/RouteConstraints/GuidConstraint.cs
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            var value = values[parameterName];
            Guid guid;
            var result = value is RouteParameter || value is Guid || (value is string && Guid.TryParse((string)value, out guid));
            return result;
        }
    }
}