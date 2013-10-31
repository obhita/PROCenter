namespace ProCenter.Mvc.Infrastructure.Security
{
    using System.Linq;
    using System.Security.Claims;
    using Common;
    using Pillar.Security.AccessControl;

    /// <summary>
    /// This class serves the permission information for current user.
    /// </summary>
    public class CurrentUserPermissionService : ICurrentUserPermissionService
    {
        #region Constants and Fields

        private readonly ICurrentClaimsPrincipalService _currentClaimsPrincipalService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserPermissionService"/> class.
        /// </summary>
        /// <param name="currentClaimsPrincipalService">The current claims principal service.</param>
        public CurrentUserPermissionService(ICurrentClaimsPrincipalService currentClaimsPrincipalService)
        {
            _currentClaimsPrincipalService = currentClaimsPrincipalService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the current user has the specified <see cref="Permission"/>.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>
        ///   <c>true</c> if the user has been granted the specified <see cref="ProCenter.Mvc.Infrastructure.Permission"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool DoesUserHavePermission(Permission permission)
        {
            var claimsPrincipal = _currentClaimsPrincipalService.GetCurrentPrincipal();
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            var hasClaim = claimsIdentity.Claims.Any(
                                                     c =>
                                                     c.Type == ProCenterClaimType.PermissionClaimType && c.Value == permission.Name);

            return hasClaim;
        }

        #endregion
    }
}