#region Licence Header
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
