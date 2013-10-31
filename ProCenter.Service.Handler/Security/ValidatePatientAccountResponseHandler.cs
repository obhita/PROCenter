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
namespace ProCenter.Service.Handler.Security
{
    #region Using Statements

    using Common;
    using Domain.PatientModule;
    using Domain.SecurityModule;
    using Service.Message.Security;

    #endregion

    public class ValidatePatientAccountResponseHandler : ServiceRequestHandler<ValidatePatientAccountRequest, ValidatePatientAccountResponse>
    {
        #region Fields

        private readonly IPatientRepository _patientRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        public ValidatePatientAccountResponseHandler ( ISystemAccountRepository systemAccountRepository, IPatientRepository patientRepository )
        {
            _systemAccountRepository = systemAccountRepository;
            _patientRepository = patientRepository;
        }

        #endregion

        #region Methods

        protected override void Handle ( ValidatePatientAccountRequest request, ValidatePatientAccountResponse response )
        {
            var systemAccount = _systemAccountRepository.GetByKey ( request.SystemAccountKey );
            var patient = _patientRepository.GetByKey ( systemAccount.PatientKey.Value );
            var validationResult = patient.ValidateInfo ( systemAccount, request.PatientIdentifier, request.DateOfBirth );
            if ( validationResult == ValidationStatus.Locked )
            {
                response.IsLocked = true;
            }
            if ( validationResult == ValidationStatus.Valid )
            {
                response.Validated = true;
            }
        }

        #endregion
    }
}