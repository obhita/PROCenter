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
namespace ProCenter.Service.Handler.Patient
{
    #region Using Statements

    using Common;
    using Domain.CommonModule;
    using Domain.PatientModule;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Patient;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Save Patient request handler.
    /// </summary>
    public class SavePatientDtoRequestHandler : ServiceRequestHandler<SaveDtoRequest<PatientDto>, SaveDtoResponse<PatientDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;
        private readonly IPatientRepository _patientRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SavePatientDtoRequestHandler" /> class.
        /// </summary>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        public SavePatientDtoRequestHandler ( IPatientRepository patientRepository, ILookupProvider lookupProvider )
        {
            _patientRepository = patientRepository;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( SaveDtoRequest<PatientDto> request, SaveDtoResponse<PatientDto> response )
        {
            var patient = _patientRepository.GetByKey ( request.DataTransferObject.Key );
            if ( patient != null )
            {
                patient.ReviseName ( request.DataTransferObject.Name );
                patient.ReviseDateOfBirth ( request.DataTransferObject.DateOfBirth );
                patient.ReviseGender ( _lookupProvider.Find<Gender> ( request.DataTransferObject.Gender.Code ) );
                if ( request.DataTransferObject.Ethnicity != null && !string.IsNullOrEmpty ( request.DataTransferObject.Ethnicity.Code ) )
                {
                    patient.ReviseEthnicity ( _lookupProvider.Find<Ethnicity> ( request.DataTransferObject.Ethnicity.Code ) );
                }
                if ( request.DataTransferObject.Religion != null && !string.IsNullOrEmpty ( request.DataTransferObject.Religion.Code ) )
                {
                    patient.ReviseReligion ( _lookupProvider.Find<Religion> ( request.DataTransferObject.Religion.Code ) );
                }
                patient.ReviseEmail(string.IsNullOrWhiteSpace(request.DataTransferObject.Email) ? null : new Email(request.DataTransferObject.Email));

                response.DataTransferObject = Mapper.Map<Patient, PatientDto> ( patient );
            }
        }

        #endregion
    }
}