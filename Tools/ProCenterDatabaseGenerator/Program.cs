namespace ProCenterDatabaseGenerator
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using Dapper;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;
    using Pillar.Security.AccessControl;
    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.Nida;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.SecurityModule;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.Security;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using ProCenter.Mvc.Infrastructure.Boostrapper;
    using ProCenter.Mvc.Infrastructure.Permission;
    using ProCenter.Mvc.PermissionDescriptor;
    using ProCenter.Primitive;
    using ProCenter.Service.Message.Assessment;
    using Raven.Abstractions.Data;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Extensions;

    #endregion

    internal class Program
    {
        #region Constants

        private const string DatabaseName = "PROCenter";

        #endregion

        #region Static Fields

        private static readonly byte[] EncryptionKey = new byte[]
            {
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
            };

        #endregion

        #region Methods

        private static void InitializeDatabase(string connectionString)
        {
            using (var documentStore = new DocumentStore())
            {
                documentStore.Conventions.ShouldCacheRequest = url => false;
                if (connectionString == null)
                {
                    documentStore.ConnectionStringName = "RavenDbSystem";
                }
                else
                {
                    documentStore.ParseConnectionString(connectionString);
                }
                documentStore.Initialize();

                documentStore.DatabaseCommands.EnsureDatabaseExists(DatabaseName);

                var proCenterDbCommands = documentStore.DatabaseCommands.ForDatabase(DatabaseName);
                var allDocuments = proCenterDbCommands.GetIndex("AllDocuments");
                if (allDocuments == null)
                {
                    proCenterDbCommands.PutIndex("AllDocuments",
                                                 new IndexDefinition {Map = "from doc in docs select new {doc}"});
                }

                proCenterDbCommands.DeleteByIndex("AllDocuments", new IndexQuery {PageSize = int.MaxValue}, true);
            }
        }

        private static void Main(string[] args)
        {
            var assessmentDefinitions = typeof (Frequency).Assembly.GetTypes()
                                                          .Where(t => t.IsSubclassOf(typeof (AssessmentDefinition)))
                                                          .Select(assessmentDefType => Activator.CreateInstance(assessmentDefType) as AssessmentDefinition).ToList();

            Console.WriteLine("Data update starting ......");

            InitializeDatabase(args.Length > 0 ? args[0] : null);

            Console.WriteLine("Database cleared ......");
            
            Console.WriteLine("Bootstrapping ......");
            new Bootstrapper().Run();

            var assessmentDefinitionDtos = new List<AssessmentDefinitionDto>();
            foreach (var assessmentDefinition in assessmentDefinitions)
            {
                var assessmentDef = new AssessmentDefinition(assessmentDefinition.CodedConcept);
                assessmentDefinitionDtos.Add(new AssessmentDefinitionDto
                    {
                        Key = assessmentDef.Key,
                        AssessmentName = assessmentDefinition.CodedConcept.Name,
                        AssessmentCode = assessmentDefinition.CodedConcept.Code
                    });
                foreach (var itemDefinition in assessmentDefinition.ItemDefinitions)
                {
                    assessmentDef.AddItemDefinition(itemDefinition);
                }

                //SaveDefinitionToEventStore(wireup, assessmentDefinition);
                Console.WriteLine("Added assessment definition: {0} ......", assessmentDefinition.CodedConcept.Name);
            }

            Guid staffKey;
            var organizationkey = SetupOrganization(assessmentDefinitionDtos, out staffKey);
            
            var patientKey = AddPatients(organizationkey);
            SetupPatientPortal (organizationkey, patientKey);
            AddAssessmentReminders(assessmentDefinitionDtos, organizationkey, patientKey, staffKey);

            var unitOfWorkProvider = IoC.CurrentContainer.Resolve<IUnitOfWorkProvider>();
            unitOfWorkProvider.GetCurrentUnitOfWork().Commit();

            Console.WriteLine("Data update completed ......");
            //Console.WriteLine("Press any key to continue:");
            //Console.ReadKey();
        }

        private static Guid SetupOrganization(IEnumerable<AssessmentDefinitionDto> assessmentDefinitionDtos, out Guid staffKey)
        {
            var organization = new Organization("Safe Harbor");

            var identity = new ClaimsIdentity(new List<Claim> { new Claim(ProCenterClaimType.OrganizationKeyClaimType, organization.Key.ToString()) });
            var currentPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = currentPrincipal;

            organization.AddAddress(new OrganizationAddress(OrganizationAddressType.Office,
                                                            new Address("123 Main Street", "", "Columbia", UnitedStates.Maryland, new PostalCode("12345"))));
            organization.AddPhone(new OrganizationPhone(OrganizationPhoneType.Office, new Phone("1234567890")));
            foreach (var assessmentDefinitionDto in assessmentDefinitionDtos)
            {
                organization.AddAssessmentDefinition(assessmentDefinitionDto.Key);
            }

            {
                var staff = new Staff(organization.Key, new PersonName("Leo", "Smith"));
                staffKey = staff.Key;
                var systemAccount = new SystemAccount(organization.Key, "leo.smith@safeharbor.com", new Email("leo.smith@safeharbor.com"));
                var role = new Role("Organization Admin");
                var permissionDescriptors = IoC.CurrentContainer.ResolveAll<IPermissionDescriptor>();
                var type = typeof (BasicAccessPermissionDescriptor);
                var allPermissions = new List<Permission>();

                foreach (var resource in permissionDescriptors.OfType<IInternalPermissionDescriptor>().Where ( pd => !pd.IsInternal ).SelectMany(pd => pd.Resources))
                {
                    GetPermissionsHelper(allPermissions, resource);
                }

                if (allPermissions.Count == 0)
                {
                    allPermissions.Add(BasicAccessPermission.AccessUserInterfacePermission);
                    allPermissions.Add(PatientPermission.PatientEditPermission);
                    allPermissions.Add(PatientPermission.PatientViewPermission);
                    allPermissions.Add(StaffPermission.StaffAddRolePermission);
                    allPermissions.Add(StaffPermission.StaffCreateAccountPermission);
                    allPermissions.Add(StaffPermission.StaffEditPermission);
                    allPermissions.Add(StaffPermission.StaffLinkAccountPermission);
                    allPermissions.Add(StaffPermission.StaffRemoveRolePermission);
                    allPermissions.Add(StaffPermission.StaffViewPermission);
                    allPermissions.Add(OrganizationPermission.OrganizationEditPermission);
                    allPermissions.Add(OrganizationPermission.OrganizationViewPermission);
                    allPermissions.Add(RolePermission.RoleAddPermissionPermission);
                    allPermissions.Add(RolePermission.RoleEditPermission);
                    allPermissions.Add(RolePermission.RoleRemovePermissionPermission);
                    allPermissions.Add(RolePermission.RoleViewPermission);
                    allPermissions.Add(TeamPermission.TeamEditPermission);
                    allPermissions.Add(TeamPermission.TeamViewPermission);
                    allPermissions.Add(AssessmentPermission.AssessmentEditPermission);
                    allPermissions.Add(AssessmentPermission.AssessmentViewPermission);
                }

                foreach (var permission in allPermissions)
                {
                    role.AddPermision(permission);
                }
                systemAccount.AddRole(role.Key);
                systemAccount.AssignToStaff(staff.Key);
            }

            {
                var staff = new Staff(organization.Key, new PersonName("Cindy", "Thomas"));
                var systemAccount = new SystemAccount(organization.Key, "cindy.thomas@safeharbor1x1.com",
                                                      new Email("cindy.thomas@safeharbor1x1.com"));
                var role = new Role("Organization Viewer");
                role.AddPermision(BasicAccessPermission.AccessUserInterfacePermission);
                role.AddPermision(PatientPermission.PatientViewPermission);
                role.AddPermision(AssessmentPermission.AssessmentViewPermission);
                role.AddPermision(StaffPermission.StaffViewPermission);
                systemAccount.AddRole(role.Key);
                systemAccount.AssignToStaff(staff.Key);
            }
            return organization.Key;
        }

        private static Guid AddPatients(Guid organizationkey)
        {
            Guid patientKey;
            var patientFacory = new PatientFactory();
            {
                var patient = patientFacory.Create(organizationkey, new PersonName("O-Ren", "Z", "Ishii"), DateTime.Parse("01/01/1954"), Gender.Female);
                Console.WriteLine("Added patient: {0} {1} ......", patient.Name.FirstName, patient.Name.LastName);
                patientKey = patient.Key;
            }
            {
                var patient = patientFacory.Create(organizationkey, new PersonName("John", "B", "Smith"), DateTime.Parse("01/01/1983"), Gender.Male);
                Console.WriteLine("Added patient: {0} {1} ......", patient.Name.FirstName, patient.Name.LastName);
            }
            return patientKey;
        }

        private static void SetupPatientPortal (Guid organizationKey, Guid patientKey)
        {
            var portalRole = new Role("Patient Portal", RoleType.BuiltIn);
            portalRole.AddPermision(BasicAccessPermission.AccessUserInterfacePermission);
            portalRole.AddPermision(PortalPermission.PortalViewPermission);
            portalRole.AddPermision(PatientPermission.PatientViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentReminderViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentEditPermission);
            portalRole.AddPermision(AssessmentPermission.ReportViewPermission);

            var systemAccount = new SystemAccount(organizationKey, "oren.ishii@gmail.com", new Email("oren.ishii@gmail.com"));
            systemAccount.AssignToPatient ( patientKey );
            systemAccount.AddRole ( portalRole.Key );
        }

        private static void AddAssessmentReminders(IEnumerable<AssessmentDefinitionDto> assessmentDefinitionDtos, Guid organizationKey, Guid patientKey, Guid staffKey)
        {
            var assessmentDefDto = assessmentDefinitionDtos.First(a => a.AssessmentCode == "3254099");
            var assessmentReminder = new AssessmentReminder(organizationKey,
                                                            patientKey,
                                                            staffKey,
                                                            assessmentDefDto.Key,
                                                            "Followup",
                                                            DateTime.Now.AddDays(1),
                                                            "Test");
            assessmentReminder.ReviseReminder(1, AssessmentReminderUnit.Days, new Email("yu.mei@feisystems.com"));
            Console.WriteLine("Added Assessment Reminder: {0} ......", "Followup");
        }


        private static void GetPermissionsHelper(IList<Permission> permissions, Resource resource)
        {
            if (resource.Resources != null)
            {
                foreach (var subResource in resource.Resources)
                {
                    GetPermissionsHelper(permissions, subResource);
                }
            }
            if (!permissions.Contains(resource.Permission))
            {
                permissions.Add(resource.Permission);
            }
        }

        #endregion
    }
}