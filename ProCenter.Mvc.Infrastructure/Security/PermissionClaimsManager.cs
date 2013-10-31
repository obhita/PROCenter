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

    #endregion

    public class PermissionClaimsManager : IPermissionClaimsManager
    {
        #region Fields

        private readonly IRoleRepository _roleRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IStaffRepository _staffRepository;

        #endregion

        #region Constructors and Destructors

        public PermissionClaimsManager ( IStaffRepository staffRepository, IRoleRepository roleRepository, IPatientRepository patientRepository )
        {
            _staffRepository = staffRepository;
            _roleRepository = roleRepository;
            _patientRepository = patientRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void IssueAccountClaims ( ClaimsPrincipal claimsPrincipal, SystemAccount systemAccount )
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if ( identity != null )
            {
                identity.AddClaim ( new Claim ( ProCenterClaimType.AccountKeyClaimType, systemAccount.Key.ToString () ) );
                if ( systemAccount.OrganizationKey != Guid.Empty )
                {
                    identity.AddClaim ( new Claim ( ProCenterClaimType.OrganizationKeyClaimType, systemAccount.OrganizationKey.ToString () ) );
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
                if ( systemAccount.PatientKey != null )
                {
                    var patient = _patientRepository.GetByKey(systemAccount.PatientKey.Value);
                    if (patient == null)
                    {
                        throw new InvalidOperationException("Patient does not exist for key " + systemAccount.PatientKey);
                    }
                    identity.AddClaim(new Claim(ProCenterClaimType.PatientKeyClaimType, systemAccount.PatientKey.ToString()));
                    identity.AddClaim(new Claim(ProCenterClaimType.UserFirstNameClaimType, patient.Name.FirstName));
                    identity.AddClaim(new Claim(ProCenterClaimType.UserLastNameClaimType, patient.Name.LastName));
                }

                if ( systemAccount.Validated )
                {
                    IssueSystemAccountValidationClaim ( claimsPrincipal );
                }
                systemAccount.LogIn ();
            }
        }

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

        public void IssueSystemAccountValidationClaim ( ClaimsPrincipal claimsPrincipal )
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if ( identity != null )
            {
                identity.AddClaim ( new Claim ( ProCenterClaimType.ValidatedClaimType, true.ToString() ) );
            }
        }

        #endregion
    }
}