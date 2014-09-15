namespace ProCenter.Common.Permission
{
    #region Using Statements

    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>The system account permission class.</summary>
    public static class SystemAccountPermission
    {
        #region Static Fields

        /// <summary>
        ///     Gets the reset password permission.
        /// </summary>
        public static Permission LockAccountPermission
        {
            get { return new Permission { Name = "securitymodule/lockaccountpermission" }; }
        }

        /// <summary>
        ///     Gets the reset password permission.
        /// </summary>
        public static Permission ResetPasswordPermission
        {
            get { return new Permission { Name = "securitymodule/resetpasswordpermission" }; }
        }

        #endregion
    }
}