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
namespace ProCenter.Mvc.Tests.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Resources;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Agatha.Common;
    using App_Start;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using Mvc.Controllers;
    using Service.Message.Assessment;
    using Service.Message.Common.Lookups;
    using Service.Message.Message;
    using Service.Message.Patient;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    #endregion

    [TestClass]
    public class AssessmentControllerTest
    {
        [TestMethod]
        public void CanCreateAssessment()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var workflowKey = Guid.NewGuid();
            var assessmentInstanceKey = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<CreateAssessmentResponse>())
                                      .Returns(
                                          Task.FromResult(new CreateAssessmentResponse
                                              {
                                                  AssessmentInstanceKey = assessmentInstanceKey
                                              }));
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new AssessmentController(requestDisplatcherFactoryMock.Object,
                                                      new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Create(patientKey, Guid.NewGuid(), workflowKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var redirectToRouteResult = actionResult as RedirectToRouteResult;
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            RouteTestHelper.AssertRoute(routes, redirectToRouteResult.RouteValues, new {key = assessmentInstanceKey, patientKey, action = "Edit"});
        }

        [TestMethod]
        public void CanEditAssessment()
        {
            // Arrange
            const string assessmentName = "Test Assessment";
            var patientKey = Guid.NewGuid();
            var key = Guid.NewGuid();
            var initiatingAssessmentKey = Guid.NewGuid();
            var workflowMessageDto = new WorkflowMessageDto()
                {
                    InitiatingAssessmentKey = initiatingAssessmentKey,
                    Key = key,
                    PatientKey = patientKey,
                    RecommendedAssessmentDefinitionKey = Guid.NewGuid()
                };
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<GetSectionDtoByKeyResponse>())
                                      .Returns(
                                          Task.FromResult(new GetSectionDtoByKeyResponse
                                              {
                                                  Messages =
                                                      new List<IMessageDto>
                                                          {
                                                              workflowMessageDto
                                                          },
                                                  DataTransferObject = new SectionDto
                                                      {
                                                          AssessmentName = assessmentName
                                                      }
                                              }));
            asyncRequestDispatcherMock.Setup(rd => rd.Get<GetPatientDtoResponse>()).Returns(new GetPatientDtoResponse
                {
                    DataTransferObject =
                        new PatientDto {Key = patientKey, Gender = new LookupDto() {Code = "Male", Name = "Male"}}
                });
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var resourcesManager = new Mock<IResourcesManager>();
            resourcesManager.Setup(rm => rm.GetResourceManagerByName(assessmentName))
                            .Returns(It.IsAny<ResourceManager>);
            var controller = new AssessmentController(requestDisplatcherFactoryMock.Object, resourcesManager.Object);

            ActionResult acttionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Edit(key, patientKey);
            task.ContinueWith(result =>
                {
                    acttionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var viewResult = acttionResult as ViewResult;
            var sectionDto = viewResult.Model as SectionDto;
            Assert.IsNotNull(sectionDto);
            Assert.AreEqual(assessmentName, sectionDto.AssessmentName);
            var viewData = viewResult.ViewData;
            var messageDto = (viewData["Messages"] as List<IMessageDto>)[0] as WorkflowMessageDto;
            Assert.AreEqual(initiatingAssessmentKey, messageDto.InitiatingAssessmentKey);
            Assert.AreEqual(patientKey, messageDto.PatientKey);
            Assert.AreEqual("Male", (viewData["Patient"] as PatientDto).Gender.Code);
        }

        [TestMethod]
        public void CanEditAssessmentExpectJsonResult()
        {
            // Arrange
            const bool canSubmit = true;
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<SaveAssessmentItemResponse>())
                                      .Returns(
                                          Task.FromResult(new SaveAssessmentItemResponse
                                              {
                                                  CanSubmit = canSubmit
                                              }));

            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new AssessmentController(requestDisplatcherFactoryMock.Object,
                                                      new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Edit(Guid.NewGuid(), Guid.NewGuid(), "123345", "");
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var jsonResult = actionResult as JsonResult;
            var data = jsonResult.Data;
            var canSubmitResult = (bool) data.GetType().GetProperty("CanSubmit").GetValue(data, null);
            Assert.AreEqual(canSubmit, canSubmitResult);
        }

        [TestMethod]
        public void CanSubmitAssessment()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var key = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<SubmitAssessmentResponse>())
                                      .Returns(
                                          Task.FromResult(new SubmitAssessmentResponse
                                              {
                                                  ScoreDto = new ScoreDto()
                                                      {
                                                          Value = 1
                                                      }
                                              }));

            asyncRequestDispatcherMock.Setup(rd => rd.Get<GetPatientDtoResponse>()).Returns(new GetPatientDtoResponse
                {
                    DataTransferObject =
                        new PatientDto {Key = patientKey, Gender = new LookupDto() {Code = "Male", Name = "Male"}}
                });
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new AssessmentController(requestDisplatcherFactoryMock.Object,
                                                      new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Submit(key, patientKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var partialViewResult = actionResult as PartialViewResult;
            Assert.AreEqual("ScoreHeader", partialViewResult.ViewName);
            var scoreDto = (partialViewResult.Model as ScoreHeaderViewModel).Score;
            Assert.AreEqual(1, scoreDto.Value);
            Assert.AreEqual("Male", (partialViewResult.ViewData["Patient"] as PatientDto).Gender.Code);
        }
    }
}