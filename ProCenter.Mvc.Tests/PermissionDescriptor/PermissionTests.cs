namespace ProCenter.Mvc.Tests.PermissionDescriptor
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Infrastructure.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mvc.Controllers;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Tests;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.EventStore;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    #endregion

    [TestClass]
    public class PermissionTests
    {
        [TestMethod]
        public void EachPermissionResouceHasValidActionDescriptor()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Arrange
                SetServiceLocatorFixture(serviceLocatorFixture);

                //Act 
                var permissionDescriptors = IoC.CurrentContainer.ResolveAll<IPermissionDescriptor>();
                var allActionPermissions = new Dictionary<string, List<string>>();
                var resources = permissionDescriptors.SelectMany(pd => pd.Resources);
                foreach (var resource in resources)
                {
                    var controller = resource.Name;
                    var actions= new List<string>();
                    if (resource.Resources != null)
                    {
                        foreach (var subResource in resource.Resources.Where(subResource => !actions.Contains(subResource.Name)))
                        {
                            actions.Add(subResource.Name);
                        }
                    }
                    allActionPermissions.Add(controller, actions);
                }
               
                var controllers = IoC.CurrentContainer.ResolveAll<BaseController>();
                var allActions = new Dictionary<string, List<string>>();
                foreach (var controller in controllers)
                {
                    var reflectedControllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
                    var actionDescriptors = reflectedControllerDescriptor.GetCanonicalActions();
                    var actions = actionDescriptors.Select(actionDescriptor => actionDescriptor.ActionName).ToList();
                    allActions.Add(controller.GetType().FullName, actions);

                }

                //Assert
                foreach (var permissionAction in allActionPermissions)
                {
                    foreach (var value in permissionAction.Value)
                    {
                        Assert.IsTrue(allActions.Any(a => a.Key == permissionAction.Key && a.Value.Contains(value)));
                    }
                }
            }
        }

        [TestMethod]
        public void EachActionMethodHasPermissionResource()
        {
        }

        private static void SetServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IAsyncRequestDispatcherFactory>().Use<FakeAsyncDispatcherFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IRequestDispatcherFactory>().Use<FakeDispatcherFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IResourcesManager>().Use<ResourcesManager>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IDbConnectionFactory>().Use<FakeSqlConnectionFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ILogoutService>().Use<LogoutService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IProvidePermissions>().Use<ProCenterAccessControlManager>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICurrentUserPermissionService>().Use<CurrentUserPermissionService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICurrentClaimsPrincipalService>().Use<CurrentClaimsPrincipleService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(p => (p.FullName == null) ? false : p.FullName.Contains("ProCenter."));
                    scanner.AddAllTypesOf<IPermissionDescriptor>();
                    scanner.AddAllTypesOf<BaseController>();
                }));
        }
    }

    public class FakeAsyncDispatcherFactory : IAsyncRequestDispatcherFactory
    {
        public Agatha.Common.IAsyncRequestDispatcher CreateAsyncRequestDispatcher()
        {
            throw new NotImplementedException();
        }
    }

    public class FakeDispatcherFactory : IRequestDispatcherFactory
    {
        public IRequestDispatcher CreateRequestDispatcher()
        {
            throw new NotImplementedException();
        }
    }

    public class FakeSqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            var sqlConnection = new SqlConnection();
            return sqlConnection;
        }
    }
}