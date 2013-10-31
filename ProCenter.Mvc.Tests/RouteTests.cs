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