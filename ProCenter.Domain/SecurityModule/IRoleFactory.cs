namespace ProCenter.Domain.SecurityModule
{
    #region

    using System;

    #endregion

    public interface IRoleFactory
    {
        Role Create(string name, RoleType roleType = RoleType.UserDefined);
    }
}