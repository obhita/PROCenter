namespace ProCenter.Mvc.Tests.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Script.Serialization;
    using Agatha.Common;
    using App_Start;
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Mvc.Controllers;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Common;
    using Service.Message.Common.Lookups;
    using Service.Message.Patient;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    #endregion

    [TestClass]
    public class PatientControllerTest
    {
        private readonly DateTime _dateOfBirth = new DateTime(1980, 1, 1);
        private readonly LookupDto _genderMale = new LookupDto {Code = "Male", Name = "Male"};

        [TestMethod]
        public void CanCreatePatient()
        {
            // Arrange
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAllAsync()).Returns(Task.FromResult(new {}));
            asyncRequestDispatcherMock.SetupGet(rd => rd.Responses)
                                      .Returns(new List<Response>
                                          {
                                              new GetLookupsByCategoryResponse
                                                  {
                                                      Category = "Gender",
                                                      Lookups = new List<LookupDto> {_genderMale}
                                                  }
                                          });

            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new PatientController(requestDisplatcherFactoryMock.Object, new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Create();
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var partialViewResult = actionResult as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.IsNotNull(partialViewResult.Model);
            Assert.AreEqual(typeof (PatientDto), partialViewResult.Model.GetType());
            Assert.AreEqual("Create", partialViewResult.ViewName);
            var lookupDtos = partialViewResult.ViewData["GenderLookupItems"] as List<LookupDto>;
            Assert.IsNotNull(lookupDtos);
            Assert.AreEqual("Male", lookupDtos[0].Code);
            Assert.IsNotNull(partialViewResult.ViewData["Patient"]);
        }

        [TestMethod]
        public void CanEditPatient()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<GetPatientDtoResponse>())
                                      .Returns(
                                          Task.FromResult(new GetPatientDtoResponse
                                              {
                                                  DataTransferObject =
                                                      new PatientDto
                                                          {
                                                              Key = patientKey,
                                                              DateOfBirth = _dateOfBirth,
                                                              Gender = _genderMale
                                                          }
                                              }));
            asyncRequestDispatcherMock.SetupGet(rd => rd.Responses)
                                      .Returns(new List<Response>
                                          {
                                              new GetLookupsByCategoryResponse
                                                  {
                                                      Category = "Gender",
                                                      Lookups = new List<LookupDto> {_genderMale}
                                                  }
                                          });

            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new PatientController(requestDisplatcherFactoryMock.Object, new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Edit(patientKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult);
            var patientDto = viewResult.Model as PatientDto;
            Assert.IsNotNull(patientDto);
            Assert.AreEqual(_dateOfBirth, patientDto.DateOfBirth);
            Assert.AreEqual(_genderMale, patientDto.Gender);
            var lookupDtos = viewResult.ViewData["GenderLookupItems"] as List<LookupDto>;
            Assert.IsNotNull(lookupDtos);
            Assert.AreEqual("Male", lookupDtos[0].Code);
            Assert.IsNotNull(viewResult.ViewData["Patient"]);
        }

        [TestMethod]
        public void CanEditPatientWithPostAction()
        {
            // Arrange
            var patientKey = Guid.NewGuid();
            var asyncRequestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            asyncRequestDispatcherMock.Setup(rd => rd.GetAsync<SaveDtoResponse<PatientDto>>())
                                      .Returns(
                                          Task.FromResult(new SaveDtoResponse<PatientDto>
                                              {
                                                  DataTransferObject = new PatientDto
                                                      {
                                                          Key = patientKey,
                                                          DateOfBirth = new DateTime(1980, 1, 2),
                                                          Gender = _genderMale
                                                      }
                                              }));
            var requestDisplatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDisplatcherFactoryMock.Setup(r => r.CreateRequestDispatcher())
                                         .Returns(asyncRequestDispatcherMock.Object);

            var controller = new PatientController(requestDisplatcherFactoryMock.Object, new Mock<IResourcesManager>().Object);

            var patientDto = new PatientDto
                {
                    Key = patientKey,
                    DateOfBirth = _dateOfBirth,
                    Gender = _genderMale
                };

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);


            // Act
            var task = controller.Edit(patientDto.Key, patientDto);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            // Assert
            var jsonResult = actionResult as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.IsNotNull(jsonResult.Data);
        }

        [TestMethod]
        public void Dashboard_ResultCorrect()
        {
            var patientKey = Guid.NewGuid();
            var requestDispatcherMock = new Mock<IAsyncRequestDispatcher>();
            requestDispatcherMock.Setup(rd => rd.GetAsync<GetPatientDashboardResponse>()).Returns(Task.FromResult(new GetPatientDashboardResponse()));
            requestDispatcherMock.Setup(rd => rd.GetAsync<GetPatientDtoResponse>())
                                 .Returns(Task.FromResult(new GetPatientDtoResponse {DataTransferObject = new PatientDto {Key = patientKey,}}));

            var requestDispatcherFactoryMock = new Mock<IRequestDispatcherFactory>();
            requestDispatcherFactoryMock.Setup(rf => rf.CreateRequestDispatcher()).Returns(requestDispatcherMock.Object);

            var controller = new PatientController(requestDispatcherFactoryMock.Object, new Mock<IResourcesManager>().Object);

            ActionResult actionResult = null;
            var wait = new ManualResetEvent(false);

            var task = controller.PatientFeed(patientKey);
            task.ContinueWith(result =>
                {
                    actionResult = result.Result;
                    wait.Set();
                });
            wait.WaitOne();


            var partialViewResult = actionResult as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.IsNotNull(partialViewResult.ViewData["Patient"] as PatientDto);
            Assert.AreEqual(patientKey, (partialViewResult.ViewData["Patient"] as PatientDto).Key);
        }
    }
}