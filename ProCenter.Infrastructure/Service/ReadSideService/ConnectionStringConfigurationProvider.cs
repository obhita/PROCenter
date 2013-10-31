namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using System.Configuration;
    using Pillar.Common.Utility;

    #endregion

    public class ConnectionStringConfigurationProvider : IConnectionStringConfigurationProvider
    {
        private readonly ConnectionStringSettingsCollection _connectionStrings;

        #region Fields

        #endregion

        #region Constructors and Destructors

        public ConnectionStringConfigurationProvider(ConnectionStringSettingsCollection connectionStrings)
        {
            _connectionStrings = connectionStrings;
            Check.IsNotNull(connectionStrings, "connectionStrings is required.");
        }

        #endregion

        public string GetConnectionString(string connectionStringName)
        {
            return _connectionStrings[connectionStringName].ConnectionString;
        }
    }
}