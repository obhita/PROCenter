#region License Header
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