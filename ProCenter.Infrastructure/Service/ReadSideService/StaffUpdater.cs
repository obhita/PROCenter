namespace ProCenter.Infrastructure.Service.ReadSideService
{
    using System.Data;
    using Dapper;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;
    using Primitive;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.OrganizationModule.Event;

    public class StaffUpdater : IHandleMessages<StaffCreatedEvent>,
                                IHandleMessages<StaffChangedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StaffUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(StaffChangedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, PersonName>(s=>s.Name))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    var name = (message.Value as PersonName);
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET FirstName = @FirstName, LastName = @LastName WHERE StaffKey = @StaffKey",
                        new {name.FirstName, name.LastName, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, Email>(s => s.Email))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    var email = (Email) message.Value;
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET Email = @Email WHERE StaffKey = @StaffKey",
                        new {Email = email == null ? (string) null : email.Address, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, string>(s => s.Location))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET Location = @Location WHERE StaffKey = @StaffKey",
                        new {Location = (string)message.Value, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, string>(s => s.NPI))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET NPI = @NPI WHERE StaffKey = @StaffKey",
                        new {NPI= (string)message.Value, StaffKey = message.Key});
                }
            }
        }

        public void Handle(StaffCreatedEvent message)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "INSERT INTO OrganizationModule.Staff(StaffKey, OrganizationKey, FirstName, LastName) VALUES(@StaffKey, @OrganizationKey, @FirstName, @LastName)",
                    new
                        {
                            StaffKey = message.Key,
                            message.OrganizationKey,
                            message.Name.FirstName,
                            message.Name.LastName,
                        });
            }
        }
    }
}