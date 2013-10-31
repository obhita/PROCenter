namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Security.Claims;
    using System.Web;
    using NLog;

    #endregion

    /// <summary>
    ///     This class serves the claims information for current user.
    /// </summary>
    public class CurrentClaimsPrincipleService : ICurrentClaimsPrincipalService
    {
        #region Static Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the claims principle of current user.
        /// </summary>
        /// <returns>
        ///     Returns the claims principle of current user.
        /// </returns>
        public ClaimsPrincipal GetCurrentPrincipal ()
        {
            Logger.Info (
                         "CurrentClaimsPrincipalService.GetCurrentPrincipal HttpContext.Current.User Name : " + HttpContext.Current.User.Identity.Name );
            Logger.Info (
                         "CurrentClaimsPrincipalService.GetCurrentPrincipal HttpContext.Current.User IsAuthenticated : "
                         + HttpContext.Current.User.Identity.IsAuthenticated );
            return (ClaimsPrincipal) HttpContext.Current.User;
        }

        #endregion
    }
}