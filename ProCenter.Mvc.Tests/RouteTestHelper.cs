namespace ProCenter.Mvc.Tests
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;
    using App_Start;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    #endregion

    /// <summary>
    ///     Helper class for route testing.
    /// </summary>
    public class RouteTestHelper
    {
        public const string Get = "GET";
        public const string Controller = "controller";
        public const string Action = "action";


        /// <summary>
        ///     Asserts the route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="url">The URL.</param>
        /// <param name="expectations">The expectations.</param>
        public static void AssertRoute(RouteCollection routes, string url, object expectations)
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);
            Assert.IsNotNull(routeData);
            AssertRoute(routes, routeData.Values, expectations);
        }

        /// <summary>
        ///     Asserts the route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="expectations">The expectations.</param>
        public static void AssertRoute(RouteCollection routes, RouteValueDictionary routeValues, object expectations)
        {
            foreach (var kvp in new RouteValueDictionary(expectations))
            {
                Assert.IsTrue(
                    string.Equals(kvp.Value.ToString(), routeValues[kvp.Key].ToString(),
                                  StringComparison.OrdinalIgnoreCase),
                    string.Format("Expected '{0}', not '{1}' for '{2}'.", kvp.Value, routeValues[kvp.Key], kvp.Key));
            }
        }

        /// <summary>
        ///     Creates the HTTP context.
        /// </summary>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        public static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = Get)
        {
            // create the mock request 
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            // create the mock response
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(
                It.IsAny<string>())).Returns<string>(s => s);

            // create the mock context, using the request and response
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            // return the mocked context
            return mockContext.Object;
        }

        /// <summary>
        ///     Tests the incoming route result.
        /// </summary>
        /// <param name="routeResult">The route result.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="propertySet">The property set.</param>
        /// <returns></returns>
        public static bool TestIncomingRouteResult(RouteData routeResult, string controller, string action,
                                                   object propertySet = null)
        {
            Func<object, object, bool> valCompare =
                (v1, v2) => StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;

            bool result = valCompare(routeResult.Values[Controller], controller)
                          && valCompare(routeResult.Values[Action], action);

            if (result && propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                result = propInfo.All(pi => (routeResult.Values.ContainsKey(pi.Name)
                                             && valCompare(routeResult.Values[pi.Name], pi.GetValue(propertySet, null))));
            }

            return result;
        }

        /// <summary>
        ///     Tests the route match.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeProperties">The route properties.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public static void TestRouteMatch(string url, string controller, string action, object routeProperties = null,
                                          string httpMethod = Get)
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        /// <summary>
        ///     Tests the route fail.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public static void TestRouteFail(string url, string httpMethod = Get)
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Assert
            Assert.IsTrue(result == null || result.Route == null);
        }
    }
}