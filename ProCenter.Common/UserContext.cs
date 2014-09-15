#region License Header

// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

#endregion

namespace ProCenter.Common
{
    #region Using Statements

    using System;
    using System.Security.Claims;
    using System.Threading;

    using Extension;

    using Pillar.Common.Utility;

    #endregion

    /// <summary>The user context class.</summary>
    public class UserContext
    {
        #region Fields

        private readonly ClaimsPrincipal _claimsPrincipal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext" /> class.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        public UserContext ( ClaimsPrincipal claimsPrincipal)
        {
            Check.IsNotNull ( claimsPrincipal, "Claims Principal is not defined." );
            _claimsPrincipal = claimsPrincipal;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static UserContext Current
        {
            get
            {
                return new UserContext(Thread.CurrentPrincipal as ClaimsPrincipal);
            }
        }

        /// <summary>
        /// Gets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        public string OrganizationName
        {
            get
            {
                return _claimsPrincipal.GetClaim<string> ( ProCenterClaimType.OrganizationNameClaimType );
            }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return string.Format ( "{0} {1}",
                    _claimsPrincipal.GetClaim<string> ( ProCenterClaimType.UserFirstNameClaimType ),
                    _claimsPrincipal.GetClaim<string> ( ProCenterClaimType.UserLastNameClaimType ) );
            }
        }

        /// <summary>
        /// Gets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public Guid? OrganizationKey
        {
            get { return _claimsPrincipal.GetClaim<Guid?> ( ProCenterClaimType.OrganizationKeyClaimType ); }
        }

        /// <summary>
        /// Gets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid? PatientKey
        {
            get { return _claimsPrincipal.GetClaim<Guid?> ( ProCenterClaimType.PatientKeyClaimType ); }
        }

        /// <summary>
        /// Gets the staff key.
        /// </summary>
        /// <value>
        /// The staff key.
        /// </value>
        public Guid? StaffKey
        {
            get { return _claimsPrincipal.GetClaim<Guid?> ( ProCenterClaimType.StaffKeyClaimType ); }
        }

        /// <summary>
        /// Gets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid? SystemAccountKey
        {
            get { return _claimsPrincipal.GetClaim<Guid?> ( ProCenterClaimType.AccountKeyClaimType ); }
        }

        /// <summary>
        /// Gets a value indicating whether [validated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [validated]; otherwise, <c>false</c>.
        /// </value>
        public bool Validated
        {
            get { return StaffKey != null || _claimsPrincipal.GetClaim<bool?> ( ProCenterClaimType.ValidatedClaimType ) == true; }
        }

        /// <summary>
        /// Gets the validation attempts.
        /// </summary>
        /// <value>
        /// The validation attempts.
        /// </value>
        public int ValidationAttempts
        {
            get { return _claimsPrincipal.GetClaim<int> ( ProCenterClaimType.ValidationAttemptsClaimType ); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds afailed validation attempt to claims identity.</summary>
        public void FailedValidationAttempt ()
        {
            var claimsIdentity = _claimsPrincipal.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst ( ProCenterClaimType.ValidationAttemptsClaimType );
            var attempts = 0;
            if ( claim != null )
            {
                claimsIdentity.RemoveClaim ( claim );
                attempts = int.Parse ( claim.Value );
            }
            attempts++;
            claimsIdentity.AddClaim ( new Claim ( ProCenterClaimType.ValidationAttemptsClaimType, attempts.ToString () ) );
        }

        /// <summary>Refreshes the validation attempts.</summary>
        public void RefreshValidationAttempts ()
        {
            var claimsIdentity = _claimsPrincipal.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst ( ProCenterClaimType.ValidationAttemptsClaimType );
            if ( claim != null )
            {
                claimsIdentity.RemoveClaim ( claim );
            }
            claimsIdentity.AddClaim ( new Claim ( ProCenterClaimType.ValidationAttemptsClaimType, "0" ) );
        }

        #endregion
    }
}