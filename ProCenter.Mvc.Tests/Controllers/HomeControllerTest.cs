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

namespace ProCenter.Mvc.Tests.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Agatha.Common;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Pillar.Security.AccessControl;

    using ProCenter.Common;
    using ProCenter.Mvc.Controllers;
    using ProCenter.Mvc.Infrastructure.BrowserDetection;
    using ProCenter.Mvc.Infrastructure.Security;

    #endregion

    [TestClass]
    public class HomeControllerTest
    {
        #region Public Methods and Operators

        [TestMethod]
        public void Index ()
        {
            var identity = new ClaimsIdentity ( new List<Claim> { new Claim ( ProCenterClaimType.OrganizationKeyClaimType, Guid.Empty.ToString () ) } );
            var currentPrincipal = new ClaimsPrincipal ( identity );
            Thread.CurrentPrincipal = currentPrincipal;

            var sb = GetSupportedBrowserObject ();

            // Mock the request object now
            HttpContextFactory.SetCurrentContext ( GetMockedHttpContext ());

            // Arrange
            var controller = new HomeController ( new Mock<IRequestDispatcherFactory> ().Object, new Mock<IAccessControlManager> ().Object, sb.Object, new Mock<ILogoutService>().Object );

            // Act
            var result = controller.Index () as ViewResult;

            // Assert
            Assert.IsNotNull ( result );
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the supported browser object.
        /// </summary>
        /// <returns>A mocked SupportedBrowser object</returns>
        private Mock<ISupportedBrowser> GetSupportedBrowserObject ()
        {
            var sb = new Mock<ISupportedBrowser> ();
            sb.Setup ( a => a.BrowserName ).Returns ( "IE" );
            sb.Setup ( a => a.MachineType ).Returns ( "Windows" );
            sb.Setup ( a => a.Version ).Returns ( 11 );
            sb.Setup ( a => a.SupportStatus ).Returns ( SupportedBrowser.SupportStatusEnum.Supported );
            return sb;
        }

        private HttpContextBase GetMockedHttpContext()
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
            context.Setup(ctx => ctx.Request.UserAgent).Returns(request.Object.UserAgent);
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