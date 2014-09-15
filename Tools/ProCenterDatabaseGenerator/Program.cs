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
namespace ProCenterDatabaseGenerator
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using NLog.Config;

    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Event;
    using Pillar.Domain.Primitives;
    using Pillar.FluentRuleEngine;
    using Pillar.Security.AccessControl;
    using ProCenter.Common;
    using ProCenter.Common.Permission;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.GainShortScreener;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.Nida;
    using ProCenter.Domain.Gpra;
    using ProCenter.Domain.Nih;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.Psc;
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Domain.SecurityModule;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.EventStore;
    using ProCenter.Infrastructure.Security;
    using ProCenter.Mvc.Infrastructure.Boostrapper;
    using ProCenter.Mvc.Infrastructure.Service;
    using ProCenter.Mvc.Views.Portal;
    using ProCenter.Primitive;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Report;

    using Raven.Abstractions.Data;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Extensions;

    using Gender = ProCenter.Domain.PatientModule.Gender;
    using ReportNames = ProCenter.Domain.ReportsModule.ReportNames;

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
                if (connectionString == null || connectionString == "null")
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

        private static string GetArgument ( string[] args, string argKey )
        {
            var returnVal = string.Empty;
            for ( var i = 0; i < args.Length - 1; i++ )
            {
                if ( args[i] == argKey )
                {
                    returnVal = args[i + 1];
                    break;
                }
            }
            return returnVal;
        }

        private static void Main(string[] args)
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("user-context", typeof(UserContextLayoutRenderer));
            var assessmentDefinitions = new List<AssessmentDefinition> (); 

            assessmentDefinitions.Add ( new GpraInterview () );

            Console.WriteLine("Data update starting ......");

            var connectionString = GetArgument ( args, "-c" );
            InitializeDatabase(connectionString.Length > 0 ? connectionString : null);

            Console.WriteLine("Database cleared ......");
            
            Console.WriteLine("Bootstrapping ......");
            new Bootstrapper().Run();

            AddSystemAccount ();

            //TODO: use assembly locator.
            var assessmblies = new List<Assembly>
                               {
                                   typeof(DrugUseFrequency).Assembly,
                                   typeof(GainShortScreener).Assembly,
                                   typeof(PediatricSymptonChecklist).Assembly,
                                   typeof(NihHealthBehaviorsAssessment).Assembly
                               };

            Task.Run ( () =>
                      {
                          foreach ( var assessmbly in assessmblies )
                          {
                              assessmentDefinitions.AddRange (
                                                              assessmbly
                                                                  .GetTypes ()
                                                                  .Where ( t => t.IsSubclassOf ( typeof(Assessment) ) )
                                                                  .Select (
                                                                           t =>
                                                                           {
                                                                               var assessment = Activator.CreateInstance ( t ) as Assessment;
                                                                               return assessment.CreateAssessmentDefinition ();
                                                                           } ) );
                          }
                      } ).Wait();

            var assessmentDefinitionDtos = new List<AssessmentDefinitionDto>();
            List<Guid> assessmentDefinitionKeys = new List<Guid> ();
            foreach (var assessmentDefinition in assessmentDefinitions)
            {
                var assessmentDef = new AssessmentDefinition(assessmentDefinition.CodedConcept, assessmentDefinition.ScoreType);
                assessmentDefinitionDtos.Add(new AssessmentDefinitionDto
                    {
                        Key = assessmentDef.Key,
                        AssessmentName = assessmentDefinition.CodedConcept.Name,
                        AssessmentCode = assessmentDefinition.CodedConcept.Code
                    });
                assessmentDefinitionKeys.Add ( assessmentDef.Key );
                foreach (var itemDefinition in assessmentDefinition.ItemDefinitions)
                {
                    assessmentDef.AddItemDefinition(itemDefinition);
                }

                //SaveDefinitionToEventStore(wireup, assessmentDefinition);
                Console.WriteLine("Added assessment definition: {0} ......", assessmentDefinition.CodedConcept.Name);
            }

            Guid staffKey;
            var organizationkey = SetupOrganization(assessmentDefinitionDtos, out staffKey);
            
            var patientKeys = AddPatients(organizationkey);
            var portalRole = SetupPatientPortalRole ( organizationkey );
            foreach ( var patient in patientKeys )
            {
                AssignPatientToPortal(portalRole, organizationkey, patient);
            }
            AddReportDefinitions(staffKey);
            AddAssessmentReminders(assessmentDefinitionDtos, organizationkey, patientKeys.ElementAt (0).Key, staffKey);

            var addAdditionalData =  GetArgument ( args, "-x" );
            if (addAdditionalData.Length > 0)
            {
                AddAdditionalData(organizationkey, portalRole, staffKey, assessmentDefinitionDtos);
            }
            var unitOfWorkProvider = IoC.CurrentContainer.Resolve<IUnitOfWorkProvider>();
            unitOfWorkProvider.GetCurrentUnitOfWork().Commit();

            WaitForAllToBeDispatched ();
            Console.WriteLine("Data update completed ......");
        }

        private static void AddSystemAccount ()
        {
            const string SystemAccountIdentifier = "system.admin@feisystems.com";
            var systemAccountRepository = IoC.CurrentContainer.Resolve<ISystemAccountRepository>();
            var roleFactory = IoC.CurrentContainer.Resolve<IRoleFactory>();
            var systemAccount = systemAccountRepository.GetByIdentifier(SystemAccountIdentifier);
            if (systemAccount == null)
            {
                var systemAdminRole = roleFactory.Create("System Admin", null, RoleType.Internal);
                systemAdminRole.AddPermision(SystemAdministrationPermission.SystemAdminPermission);
                systemAdminRole.AddPermision(new Permission { Name = "infrastructuremodule/accessuserinterface" });

                systemAccount = new SystemAccount(Guid.Empty, SystemAccountIdentifier, new Email(SystemAccountIdentifier));
                systemAccount.AddRole(systemAdminRole.Key);
            }
        }

        private static void AddAssessmentReminderForPatient(
            Guid assessmentDefinitionKey, 
            Guid organizationKey, 
            Guid patientKey, 
            Guid staffKey, 
            string reminderName, 
            string description, 
            string email, 
            DateTime startDateTime,
            DateTime endDateTime,
            AssessmentReminderRecurrence reminderRecurrenceType,
            AssessmentReminderUnit assessmentReminderUnit)
        {
            var assessmentReminder = new AssessmentReminder(organizationKey,
                                                            patientKey,
                                                            staffKey,
                                                            assessmentDefinitionKey,
                                                            reminderName,
                                                            startDateTime,
                                                            description,
                                                            reminderRecurrenceType,
                                                            endDateTime);
            assessmentReminder.ReviseReminder(1, assessmentReminderUnit, new Email(email));
            Console.WriteLine("Added Assessment Reminder: {0} ......", reminderName);
        }

        private static void AddAdditionalData(Guid organizationkey, Role portalRole, Guid staffKey, List<AssessmentDefinitionDto> assessmentDefinitionDtos)
        {
            var patientFactory = new PatientFactory();
            {
                //// How to add a new patient
                var patient = patientFactory.Create(organizationkey, new PersonName("Billy", "", "Bob"), DateTime.Parse("02/05/1965"), Gender.Male, new Email ( "billy.bob@email.com" ));
                Console.WriteLine("Added patient: {0} {1} ......", patient.Name.FirstName, patient.Name.LastName);

                //// How to assign this patient an account
                AssignPatientToPortal(portalRole, organizationkey, patient);
                assessmentDefinitionDtos.ForEach ( dto => CreateAssessmentForPatient ( GetAssessmentDefinitionByKey ( dto.Key ), patient )  );

                //// How to assign a reminder to this patient
                AddAssessmentReminderForPatient(
                    assessmentDefinitionDtos.First(f => f.AssessmentCode == "1000000").Key, 
                    organizationkey, 
                    patient.Key, 
                    staffKey, 
                    "Test Reminder", 
                    "Reminder Description", 
                    "joe@home.com", 
                    DateTime.Now, 
                    DateTime.Now.AddDays ( 4 ), 
                    AssessmentReminderRecurrence.Daily, 
                    AssessmentReminderUnit.Days );
            }
        }

        private static AssessmentDefinition GetAssessmentDefinitionByKey(Guid key)
        {
            var assessmentDefinitionRepository = IoC.CurrentContainer.Resolve<IAssessmentDefinitionRepository>();
            return assessmentDefinitionRepository.GetByKey(key);
        }

        private static AssessmentInstance CreateAssessmentForPatient ( AssessmentDefinition assessmentDefinition, Patient patient )
        {
            var assessmentInstanceFactory = IoC.CurrentContainer.Resolve<IAssessmentInstanceFactory>();
            var instance = assessmentInstanceFactory.Create(assessmentDefinition, patient.Key, assessmentDefinition.CodedConcept.Name);
            //instance.UpdateItem();
            Console.WriteLine("Added Assessment Instance {0} to patient: {1} ......", assessmentDefinition.CodedConcept.Name, patient.Name.FullName);
            return instance;
        }

        private static void WaitForAllToBeDispatched ()
        {
            const int TimesToTry = 100;
            var attempted = 0;
            var eventStoreFactory = IoC.CurrentContainer.Resolve<IEventStoreFactory> ();
            while ( eventStoreFactory.CachedEventStores.Any( esf => esf.Advanced.GetUndispatchedCommits ().Any( )) )
            {
                attempted++;
                if ( attempted == TimesToTry )
                {
                    break;
                }
                Thread.Sleep ( 1 );
            }
            if ( attempted == TimesToTry )
            {
                Console.WriteLine("Too many tries waiting for all commits to be dispatched, there is most likely an exception happening in the Read Side Service.");
                Console.ReadKey();
            }
        }

        private static void AddReportDefinitions ( Guid staffKey )
        {
            var reportDefinitionFactory = new ReportDefinitionFactory ( );
            reportDefinitionFactory.Create(staffKey, ReportNames.AssessmentScoreOverTime, Report.AssessmentScoreOverTime, true);
            Console.WriteLine("Added report definition: {0}......", ReportNames.AssessmentScoreOverTime);
            reportDefinitionFactory.Create(staffKey, ReportNames.PatientScoreRange, Report.PatientScoreRange, false);
            Console.WriteLine("Added report definition: {0}......", ReportNames.PatientScoreRange);
            reportDefinitionFactory.Create(staffKey, ReportNames.NotCompletedAssessment, Report.NotCompletedAssessment, false);
            Console.WriteLine("Added report definition: {0}......", ReportNames.NotCompletedAssessment);
            reportDefinitionFactory.Create(staffKey, ReportNames.PatientsWithSpecificResponse, Report.PatientsWithSpecificResponse, false);
            Console.WriteLine("Added report definition: {0}......", ReportNames.PatientsWithSpecificResponse);
            reportDefinitionFactory.Create(staffKey, ReportNames.PatientsWithSpecificResponseAcrossAssessments, Report.PatientsWithSpecificResponseAcrossAssessments, false);
            Console.WriteLine("Added report definition: {0}......", ReportNames.PatientsWithSpecificResponse);
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
                var role = new Role("Organization Admin", organization.Key);
                var permissionDescriptors = IoC.CurrentContainer.ResolveAll<IPermissionDescriptor>();
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
                    allPermissions.Add(SystemAccountPermission.LockAccountPermission);
                    allPermissions.Add(SystemAccountPermission.ResetPasswordPermission);
                    allPermissions.Add(ReportsCenterPermission.ReportsCenterViewPermission);
                    allPermissions.Add(AssessmentPermission.AssessmentReminderViewPermission);
                    allPermissions.Add(AssessmentPermission.AssessmentReminderEditPermission);
                    allPermissions.Add(AssessmentPermission.ReportEditPermission);
                    allPermissions.Add(AssessmentPermission.ReportViewPermission);
                    allPermissions.Add(PortalPermission.PortalViewPermission);
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
                var role = new Role("Organization Viewer", organization.Key);
                role.AddPermision(BasicAccessPermission.AccessUserInterfacePermission);
                role.AddPermision(PatientPermission.PatientViewPermission);
                role.AddPermision(AssessmentPermission.AssessmentViewPermission);
                role.AddPermision(StaffPermission.StaffViewPermission);
                systemAccount.AddRole(role.Key);
                systemAccount.AssignToStaff(staff.Key);
            }
            return organization.Key;
        }

        private static List<Patient> AddPatients(Guid organizationkey)
        {
            var patientKeys = new List<Patient> ();
            var patientFactory = new PatientFactory();
            {
                var patient = patientFactory.Create(organizationkey, new PersonName("O-Ren", "Z", "Ishii"), DateTime.Parse("01/01/1954"), Gender.Female, new Email("oren.ishii@gmail.com"));
                Console.WriteLine("Added patient: {0} {1} ......", patient.Name.FirstName, patient.Name.LastName);
                patientKeys.Add ( patient);
            }
            {
                var patient = patientFactory.Create(organizationkey, new PersonName("John", "B", "Smith"), DateTime.Parse("01/01/1983"), Gender.Male);
                Console.WriteLine("Added patient: {0} {1} ......", patient.Name.FirstName, patient.Name.LastName);
                patientKeys.Add ( patient );
            }
            return patientKeys;
        }

        private static Role SetupPatientPortalRole(Guid organizationKey)
        {
            var portalRole = new Role("Patient Portal", organizationKey, RoleType.BuiltIn);
            portalRole.AddPermision(BasicAccessPermission.AccessUserInterfacePermission);
            portalRole.AddPermision(PortalPermission.PortalViewPermission);
            portalRole.AddPermision(PatientPermission.PatientViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentReminderViewPermission);
            portalRole.AddPermision(AssessmentPermission.AssessmentEditPermission);
            portalRole.AddPermision(AssessmentPermission.ReportViewPermission);
            return portalRole;
        }

        private static void AssignPatientToPortal (Role portalRole, Guid organizationKey, Patient patient)
        {
            if ( patient.Email == null )
            {
                return;
            }
            var systemAccount = new SystemAccount(organizationKey, patient.Email.Address, patient.Email);
            systemAccount.AssignToPatient ( patient.Key );
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
                                                            "Test",
                                                            AssessmentReminderRecurrence.OneTime,
                                                            DateTime.Now.AddDays(1));
            assessmentReminder.ReviseReminder(1, AssessmentReminderUnit.Days, new Email("joey.fox@feisystems.com"));
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