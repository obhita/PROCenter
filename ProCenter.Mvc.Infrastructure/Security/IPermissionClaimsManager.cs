namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Security.Claims;
    using Domain.SecurityModule;

    #endregion

    /// <summary>
    ///     This interface provides ability to manage permission claims.
    /// </summary>
    public interface IPermissionClaimsManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Issues the account staff claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="systemAccount">The system account.</param>
        void IssueAccountClaims ( ClaimsPrincipal claimsPrincipal, SystemAccount systemAccount );

        /// <summary>
        ///     Issues the system permission claims.
        /// </summary>
        /// <param name="claimsPrincipal">
        ///     The claims principal.
        /// </param>
        /// <param name="systemAccount">
        ///     The system account.
        /// </param>
        void IssueSystemPermissionClaims ( ClaimsPrincipal claimsPrincipal, SystemAccount systemAccount );

        /// <summary>
        /// Issues the system account validation claim.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        void IssueSystemAccountValidationClaim ( ClaimsPrincipal claimsPrincipal );

        #endregion
    }
}