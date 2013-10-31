namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Pillar.Common.Configuration;

    #endregion

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConnectionStringConfigurationProvider _connectionStringConfigurationProvider;

        public SqlConnectionFactory(IConnectionStringConfigurationProvider connectionStringConfigurationProvider)
        {
            _connectionStringConfigurationProvider = connectionStringConfigurationProvider as IConnectionStringConfigurationProvider;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _connectionStringConfigurationProvider.GetConnectionString("ProCenterSqlDatabase");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Connection string not found in the configuration file.");
            }
            var sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception exception)
            {
                throw new Exception( "An error occurred while connecting to the database. See innerException for details.", exception);
            }
            return sqlConnection;
        }
    }
}