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

namespace ProCenter.Mvc.Infrastructure.Tests.BrowserDetection
{
    #region Using Statements

    using System;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ProCenter.Mvc.Infrastructure.BrowserDetection;

    #endregion

    [TestClass]
    public class SupportedBrowserTest
    {
        #region Public Methods and Operators

        [TestMethod]
        public void GetListValidMachineNameReturnsList ()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser();
            var l = sb.GetList ( "Windows" );

            Assert.IsTrue ( l.Count == 3);
        }

        [TestMethod]
        public void GetListInValidMachineNameReturnsEmptyList()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser ();
            var l = sb.GetList("Wrong Data");

            Assert.IsTrue(l.Count == 0);
        }

        [TestMethod]
        public void GetMessageBlockedStatusReturnsString()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser { SupportStatus = SupportedBrowser.SupportStatusEnum.Blocked };
            var message = sb.GetMessage ();

            Assert.IsTrue(!string.IsNullOrWhiteSpace ( message ));
        }

        [TestMethod]
        public void GetMessageWarningStatusReturnsString()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser { SupportStatus = SupportedBrowser.SupportStatusEnum.Warning };
            var message = sb.GetMessage();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(message));
        }

        [TestMethod]
        public void GetMessageSupportedStatusReturnsEmptyString()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser { SupportStatus = SupportedBrowser.SupportStatusEnum.Supported };
            var message = sb.GetMessage();

            Assert.IsTrue(string.IsNullOrWhiteSpace(message));
        }

        [TestMethod]
        public void GetMessageUnknownStatusReturnsEmptyString()
        {
            // Mock the request object now
            HttpContextFactory.SetCurrentContext(GetMockedHttpContext());
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser { SupportStatus = SupportedBrowser.SupportStatusEnum.Unknown };
            var message = sb.GetMessage();

            Assert.IsTrue(string.IsNullOrWhiteSpace(message));
        }

        [TestMethod]
        public void VerifyUnsupportedBrowserVersion()
        {
            // Mock the request object now
            var requestContext = new Mock<RequestContext>();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Request.Browser).Returns(request.Object.Browser);
            context.Setup(ctx => ctx.Request.UserAgent).Returns("WINDOWS");
            context.Setup(ctx => ctx.Request.Browser.Version).Returns("8.0");
            context.Setup(ctx => ctx.Request.Browser.Browser).Returns("IE");

            HttpContextFactory.SetCurrentContext(context.Object);
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser();

            Assert.IsTrue(sb.SupportStatus != SupportedBrowser.SupportStatusEnum.Supported );
        }

        [TestMethod]
        public void VerifySupportedBrowserVersion()
        {
            // Mock the request object now
            var requestContext = new Mock<RequestContext>();
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Request.Browser).Returns(request.Object.Browser);
            context.Setup(ctx => ctx.Request.UserAgent).Returns("WINDOWS");
            context.Setup(ctx => ctx.Request.Browser.Version).Returns("9.0");
            context.Setup(ctx => ctx.Request.Browser.Browser).Returns("IE");

            HttpContextFactory.SetCurrentContext(context.Object);
            XmlsDataFactory.SetXmlData(GetXmlData());
            var sb = new SupportedBrowser();

            Assert.IsTrue(sb.SupportStatus == SupportedBrowser.SupportStatusEnum.Supported);
        }

        private static string GetXmlData ()
        {
            return
                "<supportedbrowsers>\r\n  <supported>\r\n   "
              + "<IE displayName=\"Internet Explorer\" MachineType=\"Windows\" MinVersion=\"9.0\" MaxVersion=\"12.0\" link=\"http://windows.microsoft.com/en-us/internet-explorer/download-ie\" />\r\n    "
              + "<Firefox displayName=\"Mozilla Firefox\" MachineType=\"Windows, Mac\" MinVersion=\"7.0\" MaxVersion=\"29.0\" link=\"http://www.mozilla.org/en-US/firefox/new/\" />\r\n    "
              + "<Chrome displayName=\"Google Chrome\" MachineType=\"Windows, Mac\" MinVersion=\"10.0\" MaxVersion=\"36.0\" link=\"https://www.google.com/intl/en/chrome/browser/\" />\r\n    "
              + "<Safari displayName=\"Safari\" MachineType=\"Mac\" MinVersion=\"5.0\" MaxVersion=\"6.0\" link=\"http://support.apple.com/downloads/#safari\" />\r\n  "
              + "</supported>\r\n</supportedbrowsers>";
        }

        private static HttpContextBase GetMockedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Request.Browser).Returns(request.Object.Browser);
            context.Setup ( ctx => ctx.Request.UserAgent ).Returns ( request.Object.UserAgent);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("test");
            request.Setup(req => req.Url).Returns(new Uri("http://www.google.com"));
            request.Setup(req => req.RequestContext).Returns(requestContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(new RouteData());
            request.SetupGet(req => req.Headers).Returns(new NameValueCollection());

            return context.Object;
        }

        #endregion
    }
}