namespace ProCenter.Common
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using Pillar.Common.Utility;

    public class UserContext
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        
        public UserContext(ClaimsPrincipal claimsPrincipal)
        {
            Check.IsNotNull(claimsPrincipal, "Claims Principal is not defined.");
            _claimsPrincipal = claimsPrincipal;
        }

        public Guid? SystemAccountKey { get { return _claimsPrincipal.GetClaim<Guid?> ( ProCenterClaimType.AccountKeyClaimType ); } }
        public Guid? OrganizationKey { get { return _claimsPrincipal.GetClaim<Guid?>(ProCenterClaimType.OrganizationKeyClaimType); } }
        public Guid? StaffKey { get { return _claimsPrincipal.GetClaim<Guid?>(ProCenterClaimType.StaffKeyClaimType); } }
        public Guid? PatientKey { get { return _claimsPrincipal.GetClaim<Guid?>(ProCenterClaimType.PatientKeyClaimType); } }
        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}",
                    _claimsPrincipal.GetClaim<string>(ProCenterClaimType.UserFirstNameClaimType),
                    _claimsPrincipal.GetClaim<string>(ProCenterClaimType.UserLastNameClaimType));
            }
        }
        public bool Validated { get { return StaffKey != null || _claimsPrincipal.GetClaim<bool?>(ProCenterClaimType.ValidatedClaimType) == true; } }

        public int ValidationAttempts { get { return _claimsPrincipal.GetClaim<int> ( ProCenterClaimType.ValidationAttemptsClaimType ); } }

        public void FailedValidationAttempt ()
        {
            var claimsIdentity = _claimsPrincipal.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst ( ProCenterClaimType.ValidationAttemptsClaimType );
            var attempts = 0;
            if ( claim != null )
            {
                claimsIdentity.RemoveClaim ( claim );
                attempts = int.Parse(claim.Value);
            }
            attempts++;
            claimsIdentity.AddClaim ( new Claim ( ProCenterClaimType.ValidationAttemptsClaimType, attempts.ToString() ) );
        }

        public void RefreshValidationAttempts ()
        {
            var claimsIdentity = _claimsPrincipal.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ProCenterClaimType.ValidationAttemptsClaimType);
            if (claim != null)
            {
                claimsIdentity.RemoveClaim(claim);
            }
            claimsIdentity.AddClaim(new Claim(ProCenterClaimType.ValidationAttemptsClaimType, "0"));
        }

        public static UserContext Current
        {
            get
            {
                return new UserContext ( Thread.CurrentPrincipal as ClaimsPrincipal );
            }
        }
    }
}