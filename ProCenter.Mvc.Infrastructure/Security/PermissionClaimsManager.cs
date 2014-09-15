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

namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Security.Claims;
    using Common;
    using Domain.OrganizationModule;
    using Domain.PatientModule;
    using Domain.SecurityModule;

    using Raven.Client.Connection.Async;

    #endregion

    /// <summary>The permission claims manager class.</summary>
    public class PermissionClaimsManager : IPermissionClaimsManager
    {
        #region Fields

        private readonly IPatientRepository _patientRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionClaimsManager" /> class.
        /// </summary>
        /// <param name="staffRepository">The staff repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="organizationRepository">The organization repository.</param>
        public PermissionClaimsManager ( 
            IStaffRepository staffRepository, 
            IRoleRepository roleRepository, 
            IPatientRepository patientRepository,
            IOrganizationRepository organizationRepository)
        {
            _staffRepository = staffRepository;
            _roleRepository = roleRepository;
            _patientRepository = patientRepository;
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Issues the account staff claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="systemAccount">The system account.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Staff does not exist for key  + systemAccount.StaffKey
        /// or
        /// Patient does not exist for key  + systemAccount.PatientKey.
        /// </exception>
        public void IssueAccountClaims ( ClaimsPrincipal claimsPrincipal, SystemAccount systemAccount )
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if ( identity != null )
            {
                identity.AddClaim ( new Claim ( ProCenterClaimType.AccountKeyClaimType, systemAccount.Key.ToString () ) );
                if ( systemAccount.OrganizationKey != Guid.Empty )
                {
                    identity.AddClaim ( new Claim ( ProCenterClaimType.OrganizationKeyClaimType, systemAccount.OrganizationKey.ToString () ) );
                    identity.AddClaim(new Claim(ProCenterClaimType.OrganizationNameClaimType, GetOrganizationName(systemAccount.OrganizationKey)));
                }
                var emailClaim = identity.Claims.FirstOrDefault ( c => c.Type == ClaimTypes.Email );
                if ( emailClaim != null )
                {
                    identity.RemoveClaim ( emailClaim );
                }
                identity.AddClaim ( new Claim ( ClaimTypes.Email, systemAccount.Email.Address ) );

                if ( systemAccount.StaffKey != null )
                {
                    var staff = _staffRepository.GetByKey ( systemAccount.StaffKey.Value );
                    if ( staff == null )
                    {
                        throw new InvalidOperationException ( "Staff does not exist for key " + systemAccount.StaffKey );
                    }
                    identity.AddClaim ( new Claim ( ProCenterClaimType.StaffKeyClaimType, systemAccount.StaffKey.ToString () ) );
                    identity.AddClaim ( new Claim ( ProCenterClaimType.UserFirstNameClaimType, staff.Name.FirstName ) );
                    identity.AddClaim ( new Claim ( ProCenterClaimType.UserLastNameClaimType, staff.Name.LastName ) );
                    systemAccount.Validate ();
                }
                else if ( systemAccount.PatientKey != null )
                {
                    var patient = _patientRepository.GetByKey ( systemAccount.PatientKey.Value );
                    if ( patient == null )
                    {
                        throw new InvalidOperationException ( "Patient does not exist for key " + systemAccount.PatientKey );
                    }
                    identity.AddClaim ( new Claim ( ProCenterClaimType.PatientKeyClaimType, systemAccount.PatientKey.ToString () ) );
                    identity.AddClaim ( new Claim ( ProCenterClaimType.UserFirstNameClaimType, patient.Name.FirstName ) );
                    identity.AddClaim ( new Claim ( ProCenterClaimType.UserLastNameClaimType, patient.Name.LastName ) );
                }
                else
                {
                    identity.AddClaim ( 
                        new Claim ( ProCenterClaimType.UserFirstNameClaimType, 
                            systemAccount.Identifier.Substring ( 0, systemAccount.Identifier.IndexOf ( '@' ) ) ) );
                }

                if ( systemAccount.Validated )
                {
                    IssueSystemAccountValidationClaim ( claimsPrincipal );
                }
                systemAccount.LogIn ();
            }
        }

        /// <summary>
        /// Issues the system account validation claim.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        public void IssueSystemAccountValidationClaim ( ClaimsPrincipal claimsPrincipal )
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if ( identity != null )
            {
                identity.AddClaim ( new Claim ( ProCenterClaimType.ValidatedClaimType, true.ToString () ) );
            }
        }

        /// <summary>
        /// Issues the system permission claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="systemAccount">The system account.</param>
        public void IssueSystemPermissionClaims ( ClaimsPrincipal claimsPrincipal, SystemAccount systemAccount )
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if ( identity != null )
            {
                foreach ( var roleKey in systemAccount.RoleKeys )
                {
                    var role = _roleRepository.GetByKey ( roleKey );
                    foreach ( var permission in role.Permissions )
                    {
                        identity.AddClaim ( new Claim ( ProCenterClaimType.PermissionClaimType, permission.Name ) );
                    }
                }
            }
        }

        #endregion

        private string GetOrganizationName ( Guid organizationKey )
        {
            var organization = _organizationRepository.GetByKey ( organizationKey );
            if ( organization != null )
            {
                return organization.Name;
            }
            return string.Empty;
        }
    }
}