namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using Dapper;
    using Pillar.Common.Utility;
    using Primitive;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;

    #endregion

    /// <summary>
    ///     Handles updating patient table.
    /// </summary>
    public class PatientUpdater : IHandleMessages<PatientCreatedEvent>,
                                  IHandleMessages<PatientChangedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PatientUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(PatientChangedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<Patient, Gender>(p => p.Gender))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE PatientModule.Patient SET GenderCode = @GenderCode WHERE PatientKey=@PatientKey",
                        new {GenderCode = (message.Value as Lookup).CodedConcept.Code, PatientKey = message.Key});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName>(p => p.Name))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var name = (message.Value as PersonName);
                    connection.Execute(
                        "UPDATE PatientModule.Patient SET FirstName = @FirstName, LastName = @LastName WHERE PatientKey=@PatientKey",
                        new {name.FirstName, name.LastName, PatientKey = message.Key});
                }
            }
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(PatientCreatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "INSERT INTO PatientModule.Patient VALUES(@PatientKey, @OrganizationKey, @GenderCode, @FirstName, @LastName, @UniqueIdentifier)",
                    new
                        {
                            PatientKey = message.Key,
                            message.OrganizationKey,
                            message.Name.FirstName,
                            message.Name.LastName,
                            GenderCode = message.Gender.CodedConcept.Code,
                            message.UniqueIdentifier,
                        });
            }
        }

        #endregion
    }
}