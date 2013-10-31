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
    using Service.Message.Common;
    using Service.Message.Patient;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Create patient request handler.
    /// </summary>
    public class CreatePatientRequestHandler : ServiceRequestHandler<CreatePatientRequest, SaveDtoResponse<PatientDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreatePatientRequestHandler" /> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public CreatePatientRequestHandler ( ILookupProvider lookupProvider )
        {
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( CreatePatientRequest request, SaveDtoResponse<PatientDto> response )
        {
            var patientFactory = new PatientFactory ();
            var patient = patientFactory.Create (request.PatientDto.OrganizationKey, request.PatientDto.Name, request.PatientDto.DateOfBirth, _lookupProvider.Find<Gender> ( request.PatientDto.Gender.Code ) );

            if ( patient != null )
            {
                if ( request.PatientDto.Religion != null && !string.IsNullOrEmpty ( request.PatientDto.Religion.Code ) )
                {
                    patient.ReviseReligion ( _lookupProvider.Find<Religion> ( request.PatientDto.Religion.Code ) );
                }
                if ( request.PatientDto.Ethnicity != null && !string.IsNullOrEmpty ( request.PatientDto.Ethnicity.Code ) )
                {
                    patient.ReviseEthnicity ( _lookupProvider.Find<Ethnicity> ( request.PatientDto.Ethnicity.Code ) );
                }

                var patientDto = Mapper.Map<Patient, PatientDto> ( patient );

                response.DataTransferObject = patientDto;
            }
        }

        #endregion
    }
}