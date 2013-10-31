namespace ProCenter.Infrastructure.Service.ReadSideService
{
    public interface IConnectionStringConfigurationProvider
    {
        string GetConnectionString(string connectionStringName);
    }
}