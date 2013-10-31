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
namespace ProCenter.Mvc.Tests.Controllers
{
    #region Using Statements

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Mvc.Controllers;
    using Service.Message.Message;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    #endregion

    [TestClass]
    public class WorkflowMessageControllerTest
    {
        [TestMethod]
        public void CanAdministerAssessment()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var assessmentDefinitionKey = Guid.NewGuid();
            var workflowKey = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<WorkflowMessageStatusChangedResponse>())
                                      .Returns(Task.FromResult(new WorkflowMessageStatusChangedResponse()));
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new WorkflowMessageController(requestDisplatcherFactoryMock.Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.AdministerAssessment(Guid.NewGuid(), patientKey, assessmentDefinitionKey, workflowKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var redirectToRouteResult = actionResult as RedirectToRouteResult;
            var routeValues = redirectToRouteResult.RouteValues;
            Assert.AreEqual(patientKey, routeValues["patientKey"]);
            Assert.AreEqual(assessmentDefinitionKey, routeValues["assessmentDefinitionKey"]);
            Assert.AreEqual(workflowKey, routeValues["workflowKey"]);
            Assert.AreEqual("Create", routeValues["action"]);
            Assert.AreEqual("Assessment", routeValues["controller"]);
        }

        [TestMethod]
        public void CanRejectAssessment()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<WorkflowMessageStatusChangedResponse>())
                                      .Returns(Task.FromResult(new WorkflowMessageStatusChangedResponse()));
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new WorkflowMessageController(requestDisplatcherFactoryMock.Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Reject(Guid.NewGuid(), Guid.NewGuid (), patientKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var redirectToRouteResult = actionResult as RedirectToRouteResult;
            var routeValues = redirectToRouteResult.RouteValues;
            Assert.AreEqual(patientKey, routeValues["patientKey"]);
            Assert.AreEqual("Edit", routeValues["action"]);
            Assert.AreEqual("Assessment", routeValues["controller"]);
        }
    }
}