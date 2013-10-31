namespace ProCenter.Mvc.Tests.Controllers
{
    #region Using Statements

    using System.Web.Mvc;
    using Agatha.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Mvc.Controllers;
    using Pillar.Security.AccessControl;

    #endregion

    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController(new Mock<IRequestDispatcherFactory>().Object, new Mock<IAccessControlManager>().Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}