namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Security.Claims;
    using Domain.OrganizationModule;

    #endregion

    /// <summary>
    ///     PRO Center Claims Identity
    /// </summary>
    public class ProCenterIdentity : ClaimsIdentity
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProCenterIdentity" /> class.
        /// </summary>
        /// <param name="originalIdentity">The original identity.</param>
        /// <param name="staff">The staff.</param>
        public ProCenterIdentity ( ClaimsIdentity originalIdentity, Staff staff )
            : base ( originalIdentity )
        {
            Staff = staff;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the staff.
        /// </summary>
        /// <value>
        ///     The staff.
        /// </value>
        public Staff Staff { get; private set; }

        #endregion
    }
}