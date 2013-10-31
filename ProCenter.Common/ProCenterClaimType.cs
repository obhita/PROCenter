namespace ProCenter.Common
{
    public class ProCenterClaimType
    {
        /// <summary>
        ///   Gets the claim type for account key.
        /// </summary>
        public static readonly string AccountKeyClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/accountkey";

        /// <summary>
        ///   Gets the claim type for permission.
        /// </summary>
        public static readonly string PermissionClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/permission";

        /// <summary>
        ///   Gets the claim type for staff key.
        /// </summary>
        public static readonly string StaffKeyClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/staffkey";

        /// <summary>
        ///   Gets the claim type for patient key.
        /// </summary>
        public static readonly string PatientKeyClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/patientkey";

        /// <summary>
        ///   Gets the claim type for patient validated.
        /// </summary>
        public static readonly string ValidatedClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/validated";

        /// <summary>
        ///   Gets the claim type for patient validation attempts.
        /// </summary>
        public static readonly string ValidationAttemptsClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/validationattempts";

        /// <summary>
        ///   Gets the claim type for staff FirstName.
        /// </summary>
        public static readonly string UserFirstNameClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/userfirstname";

        /// <summary>
        ///   Gets the claim type for staff LastName.
        /// </summary>
        public static readonly string UserLastNameClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/userlastname";

        /// <summary>
        ///   Gets the claim type for staff key.
        /// </summary>
        public static readonly string OrganizationKeyClaimType = "http://schemas.obhita.org/ws/2008/06/identity/claims/organizationkey";
    }
}
