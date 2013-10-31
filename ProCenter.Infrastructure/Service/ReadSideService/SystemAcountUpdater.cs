namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using Dapper;
    using ProCenter.Domain.SecurityModule.Event;

    #endregion

    public class SystemAcountUpdater : IHandleMessages<AssignedStaffToSystemAccountEvent>,
                                       IHandleMessages<SystemAccountCreatedEvent>,
                                       IHandleMessages<SystemAccountRoleRemovedEvent>,
                                       IHandleMessages<SystemAccountRoleAddedEvent>,
                                       IHandleMessages<AssignedPatientToSystemAccountEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SystemAcountUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(AssignedStaffToSystemAccountEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "update SecurityModule.SystemAccount set StaffKey = @StaffKey where SystemAccountKey = @SystemAccountKey",
                    new
                        {
                            message.StaffKey,
                            SystemAccountKey = message.Key,
                        });
            }
        }

        public void Handle(AssignedPatientToSystemAccountEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "update SecurityModule.SystemAccount set PatientKey = @PatientKey where SystemAccountKey = @SystemAccountKey",
                    new
                        {
                            message.PatientKey,
                            SystemAccountKey = message.Key,
                        });
            }
        }

        public void Handle(SystemAccountCreatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "insert into SecurityModule.SystemAccount (SystemAccountKey, OrganizationKey, [Identifier], Email) values (@SystemAccountKey, @OrganizationKey, @Identifier, @Email)",
                    new
                        {
                            SystemAccountKey = message.Key,
                            message.OrganizationKey,
                            message.Identifier,
                            Email = message.Email.Address,
                        });
            }
        }

        public void Handle(SystemAccountRoleRemovedEvent message)
        {
        }

        public void Handle(SystemAccountRoleAddedEvent message)
        {
        }
    }
}