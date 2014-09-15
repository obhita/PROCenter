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

namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System;
    using System.Linq;

    using Dapper;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Common;
    using ProCenter.Common.Email;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Assessment;

    #endregion

    /// <summary>The submit assessment request handler class.</summary>
    public class SendEmailRequestHandler :
        ServiceRequestHandler<SendEmailRequest, SendEmailResponse>
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IDbConnectionFactory _dbConnectionFactory;

        private readonly IPatientRepository _patientRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendEmailRequestHandler" /> class.
        /// </summary>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public SendEmailRequestHandler (
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IPatientRepository patientRepository,
            IDbConnectionFactory dbConnectionFactory )
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _patientRepository = patientRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( SendEmailRequest request, SendEmailResponse response )
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey ( request.AssessmentInstanceKey );
            var patient = _patientRepository.GetByKey ( assessmentInstance.PatientKey );
            var url = string.Format ( request.AssessmentInstanceUrl + "/{0}?patientKey={1}", assessmentInstance.Key, assessmentInstance.PatientKey );
            DateTime? emailSentDate, emailFailedDate;
            string patientAccountEmail;

            using ( var connection = _dbConnectionFactory.CreateConnection () )
            {
                patientAccountEmail =
                    connection.Query<string> ( "SELECT Email FROM SecurityModule.SystemAccount WHERE PatientKey=@PatientKey", new { assessmentInstance.PatientKey } )
                              .FirstOrDefault ();
            }

            if ( patientAccountEmail != null )
            {
                var emailMessage = new EmailMessage ();
                emailMessage.ToAddresses.Add ( patientAccountEmail );
                emailMessage.Body = string.Format ( AssessmentResources.SelfAdministrableAssessmentCreatedEmailBody, patient.Name.FirstName, url );
                emailMessage.Subject = AssessmentResources.SelfAdministrableAssessmentCreatedEmailSubject;
                emailMessage.IsHtml = true;

                var emailNotifier = IoC.CurrentContainer.Resolve<IEmailNotifier> ();
                var isEmailNotificationSuccessful = emailNotifier.Send ( emailMessage );

                if ( isEmailNotificationSuccessful )
                {
                    emailSentDate = DateTime.Now;
                    emailFailedDate = null;
                }
                else
                {
                    emailSentDate = assessmentInstance.EmailSentDate;
                    emailFailedDate = DateTime.Now;
                }
            }
            else
            {
                emailSentDate = assessmentInstance.EmailSentDate;
                emailFailedDate = DateTime.Now;
            }

            assessmentInstance.UpdateEmailSentDate ( emailSentDate, emailFailedDate );
            response.EmailSentDate = emailSentDate;
            response.EmailFailedDate = emailFailedDate;
        }

        #endregion
    }
}