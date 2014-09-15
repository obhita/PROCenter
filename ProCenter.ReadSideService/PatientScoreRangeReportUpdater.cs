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

namespace ProCenter.ReadSideService
{
    #region Using Statements

    using System;
    using System.Linq;

    using Dapper;

    using Pillar.Common.Utility;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;
    using ProCenter.Primitive;
    using ProCenter.Service.Message.Report;

    #endregion

    /// <summary>The report updater class.</summary>
    public class PatientScoreRangeReportUpdater :
        IHandleMessages<AssessmentScoredEvent>,
        IHandleMessages<PatientChangedEvent>
    {
        #region Fields

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IPatientRepository _patientRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientScoreRangeReportUpdater" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public PatientScoreRangeReportUpdater (
            IDbConnectionFactory connectionFactory,
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IPatientRepository patientRepository,
            IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _connectionFactory = connectionFactory;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _patientRepository = patientRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle ( AssessmentScoredEvent message )
        {
            UpdateReportData ( message );
        }

        /// <summary>
        ///     Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle ( PatientChangedEvent message )
        {
            if ( message.Property == PropertyUtil.ExtractPropertyName<Patient, Gender> ( p => p.Gender ) )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    var lookup = message.Value as Lookup;
                    if ( lookup != null )
                    {
                        connection.Execute (
                            "UPDATE [ReportModule].[PatientScoreRangeReport] SET PatientGender = @GenderCode WHERE PatientKey=@PatientKey",
                            new { GenderCode = lookup.CodedConcept.Code, PatientKey = message.Key } );
                    }
                }
            }

            if ( message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName> ( p => p.Name ) )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    var name = ( message.Value as PersonName );
                    if ( name != null )
                    {
                        connection.Execute (
                            "UPDATE [ReportModule].[PatientScoreRangeReport] SET PatientFirstName = @FirstName, PatientLastName = @LastName WHERE PatientKey=@PatientKey",
                            new { name.FirstName, name.LastName, PatientKey = message.Key } );
                    }
                }
            }
        }

        #endregion

        #region Methods

        private string GetPreviousPatientScore ( AssessmentDefinition assessmentDefinition, Guid patientKey )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var query = "SELECT AssessmentScore FROM [ReportModule].[PatientScoreRangeReport] " +
                            "WHERE PatientKey='" + patientKey + "'" +
                            " AND AssessmentCode='" + assessmentDefinition.CodedConcept.Code + "'" +
                            " ORDER BY ScoreDate DESC";
                var reportDtos = connection.Query<PatientScoreRangeDto> ( string.Format ( query ) ).ToList ();
                if ( !reportDtos.Any () )
                {
                    return null;
                }
                var row = reportDtos.ElementAt ( 0 );
                return row.AssessmentScore;
            }
        }

        private void UpdateReportData ( AssessmentScoredEvent message )
        {
            var assessment = _assessmentInstanceRepository.GetByKey ( message.Key );
            if ( !assessment.IsSubmitted )
            {
                return;
            }
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessment.AssessmentDefinitionKey );
            if ( assessmentDefinition.ScoreType == ScoreTypeEnum.NoScore )
            {
                return;
            }
            var patient = _patientRepository.GetByKey ( assessment.PatientKey );
            var previousScore = GetPreviousPatientScore(assessmentDefinition, patient.Key);
            var scoreChanged = ReadSideService.FirstTime;
            if ( previousScore != null && assessmentDefinition.ScoreType == ScoreTypeEnum.ScoreTypeInt )
            {
                if (int.Parse(assessment.Score.Value.ToString()) > int.Parse(previousScore))
                {
                    scoreChanged = ReadSideService.Higher;
                }
                else if (int.Parse(assessment.Score.Value.ToString()) < int.Parse(previousScore))
                {
                    scoreChanged = ReadSideService.Lower;
                }
                else if (int.Parse(assessment.Score.Value.ToString()) == int.Parse(previousScore))
                {
                    scoreChanged = ReadSideService.NoChange;
                }
            }
            else if ( previousScore != null && assessmentDefinition.ScoreType == ScoreTypeEnum.ScoreTypeBoolean )
            {
                if (!bool.Parse(assessment.Score.Value.ToString ()).Equals (bool.Parse(previousScore)))
                {
                    scoreChanged = ReadSideService.Higher;
                }
                else if (bool.Parse(assessment.Score.Value.ToString ()).Equals (bool.Parse(previousScore)))
                {
                    scoreChanged = ReadSideService.NoChange;
                }
            }
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    @"INSERT INTO ReportModule.PatientScoreRangeReport (
                    AssessmentInstanceKey, PatientKey, AssessmentName, AssessmentScore, ScoreDate, 
                    PatientBirthDate, PatientFirstName, PatientLastName, PatientGender,ScoreChange, AssessmentCode) 
                    VALUES( @AssessmentInstanceKey, @PatientKey, @AssessmentName, @AssessmentScore, 
                    @ScoreDate, @PatientBirthDate, @PatientFirstName, @PatientLastName, @PatientGender, @ScoreChange, @AssessmentCode)",
                    new
                    {
                        AssessmentInstanceKey = assessment.Key,
                        assessment.PatientKey,
                        assessment.AssessmentName,
                        AssessmentScore = message.Value.ToString (),
                        ScoreDate = assessment.SubmittedDate,
                        PatientBirthDate = patient.DateOfBirth.GetValueOrDefault (),
                        PatientFirstName = patient.Name.FirstName,
                        PatientLastName = patient.Name.LastName,
                        PatientGender = patient.Gender.CodedConcept.Name,
                        ScoreChange = scoreChanged,
                        AssessmentCode = assessmentDefinition.CodedConcept.Code
                    } );
            }
        }

        #endregion
    }
}