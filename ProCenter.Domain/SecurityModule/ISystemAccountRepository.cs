namespace ProCenter.Domain.SecurityModule
{
    #region Using Statements

    using CommonModule;

    #endregion

    public interface ISystemAccountRepository : IRepository<SystemAccount>
    {
        #region Public Methods and Operators

        SystemAccount GetByIdentifier(string identifier);

        #endregion
    }
}