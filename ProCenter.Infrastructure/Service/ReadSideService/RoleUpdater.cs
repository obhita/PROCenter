namespace ProCenter.Infrastructure.Service.ReadSideService
{
    using System.Data;
    using Dapper;
    using ProCenter.Domain.SecurityModule.Event;

    public class RoleUpdater : IHandleMessages<RoleCreatedEvent>, IHandleMessages<RoleNameRevisedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RoleUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(RoleCreatedEvent message)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute("INSERT INTO SecurityModule.Role VALUES(@SystemAccountKey, @OrganizationKey, @Name, @RoleType)", new
                    {
                        SystemAccountKey = message.Key,
                        message.OrganizationKey,
                        message.Name,
                        message.RoleType,
                    });
            }
        }

        public void Handle(RoleNameRevisedEvent message)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute("UPDATE SecurityModule.Role SET Name=@Name WHERE RoleKey=@RoleKey", new
                    {
                        RoleKey = message.Key,
                        message.Name,
                    });
            }
        }
    }
}