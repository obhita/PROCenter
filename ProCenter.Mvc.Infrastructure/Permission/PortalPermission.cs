namespace ProCenter.Mvc.Infrastructure.Permission
{
    using Pillar.Security.AccessControl;

    public class PortalPermission
    {
        /// <summary>
        ///     The portal view permission
        /// </summary>
        public static Permission PortalViewPermission = new Permission { Name = "portalmodule/portalview" };
    }
}
