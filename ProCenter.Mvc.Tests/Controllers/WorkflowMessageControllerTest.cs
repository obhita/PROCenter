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