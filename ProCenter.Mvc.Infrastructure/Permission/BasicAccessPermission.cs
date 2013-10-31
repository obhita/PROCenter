namespace ProCenter.Mvc.Infrastructure.Permission
{
    #region Using Statements

    using Pillar.Security.AccessControl;

    #endregion

    public static class BasicAccessPermission
    {
        #region Static Fields

        /// <summary>
        ///     Access the user interface permission
        /// </summary>
        public static Permission AccessUserInterfacePermission = new Permission { Name = "infrastructuremodule/accessuserinterface" };

        #endregion
    }
}