namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Security.Claims;

    #endregion

    /// <summary>
    ///     This interface serves the claims information for current user.
    /// </summary>
    public interface ICurrentClaimsPrincipalService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current principal.
        /// </summary>
        /// <returns>
        ///     Returns the claims principle.
        /// </returns>
        ClaimsPrincipal GetCurrentPrincipal ();

        #endregion
    }
}