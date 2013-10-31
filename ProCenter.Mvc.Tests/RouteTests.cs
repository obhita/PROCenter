#region Licence Header
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

    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    #endregion

    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void CanMapNormalControllerActionRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/patient/index");

            var routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            RouteTestHelper.AssertRoute(routes, routeData.Values, new {controller = "patient", action = "index"});
        }

        [TestMethod]
        public void RouteHasDefaultActionWhenUrlWithoutAction()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteTestHelper.AssertRoute(routes, "~/patient", new {controller = "patient", action = "index"});
        }

        [TestMethod]
        public void TestOutgoingRoutes()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var context = new RequestContext(RouteTestHelper.CreateHttpContext(), new RouteData());

            var result = UrlHelper.GenerateUrl(null, "Index", "Home", null, routes, context, true);
            var routeValues = new RouteValueDictionary {{"key", "TheKey"}};
            string result2 = UrlHelper.GenerateUrl(null, "Index", "Home", routeValues, routes, context, true);

            Assert.AreEqual("/", result);
            Assert.AreEqual("/Home/Index/TheKey", result2);
        }

        [TestMethod]
        public void MatchRoutes()
        {
            RouteTestHelper.TestRouteMatch("~/Assessment/Edit/1", "Assessment", "Edit");
            RouteTestHelper.TestRouteMatch("~/Patient/Edit/1", "Patient", "Edit");
            RouteTestHelper.TestRouteMatch("~/Assessment/Create", "Assessment", "Create");
            RouteTestHelper.TestRouteMatch("~/Patient/Create", "Patient", "Create");
        }

        [TestMethod]
        public void RouteForEmbeddedResource()
        {
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("∼/handler.axd");
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var routeData = routes.GetRouteData(mockContext.Object);

            Assert.IsNotNull(routeData);
            Assert.IsInstanceOfType(routeData.RouteHandler, typeof (StopRoutingHandler));
        }
    }
}