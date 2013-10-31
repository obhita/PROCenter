namespace ProCenter.Mvc.Infrastructure.Permission
{
    using Pillar.Security.AccessControl;

    public static class OrganizationPermission
    {
        #region Static Fields

        /// <summary>
        ///     The organization edit permission
        /// </summary>
        public static Permission OrganizationEditPermission = new Permission
            {
                Name = "organizationmodule/organizationedit"
            };

        /// <summary>
        ///     The organization view permission
        /// </summary>
        public static Permission OrganizationViewPermission = new Permission
            {
                Name = "organizationmodule/organizationview"
            };

        #endregion
    }
}