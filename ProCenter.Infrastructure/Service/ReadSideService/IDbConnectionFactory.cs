namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System.Data;

    #endregion

    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}