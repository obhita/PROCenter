namespace ProCenter.Mvc.Infrastructure.Permission
{
    #region Using Statements

    using Pillar.Security.AccessControl;

    #endregion

    public class TeamPermission
    {
        #region Static Fields

        public static Permission TeamEditPermission = new Permission
            {
                Name = "organizationmodule/teamedit"
            };

        public static Permission TeamViewPermission = new Permission
            {
                Name = "organizationmodule/teamview"
            };

        #endregion
    }
}