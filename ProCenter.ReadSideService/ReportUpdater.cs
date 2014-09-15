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
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Domain.ReportsModule.Event;
    using ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;
    using ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport;
    using ProCenter.Service.Message.Report;

    #endregion

    /// <summary>The report updater class.</summary>
    public class ReportUpdater : IHandleMessages<ReportTemplateCreatedEvent>,
                                 IHandleMessages<ReportTemplateNameChangedEvent>,
                                 IHandleMessages<ReportTemplateReportTypeChangedEvent>,
                                 IHandleMessages<ReportTemplateParametersChangedEvent>,
                                 IHandleMessages<ReportTemplateReportStateChangedEvent>,
                                 IHandleMessages<RecentReportCreatedEvent>,
                                 IHandleMessages<ReportDefinitionCreatedEvent>
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportUpdater" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public ReportUpdater ( IDbConnectionFactory connectionFactory )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( ReportTemplateCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var parameters = (BaseReportParameters)message.Parameters;

                Guid? patientKey = parameters.PatientKey;
                if ( parameters.PatientKey == new Guid () )
                {
                    patientKey = null;
                }
                var parms = parameters.ParameterString ?? parameters.ToString ();
                connection.Execute (
                    @"INSERT INTO [ReportModule].[ReportTemplate] ([ReportTemplateKey],[SystemAccountKey],[Name],[ReportType],[Parameters],[ReportStateCode],[PatientKey],[OrganizationKey])
                        VALUES (@ReportTemplateKey,@SystemAccountKey,@Name,@ReportType,@Parameters,@ReportStateCode,@PatientKey, @OrganizationKey)",
                    new
                    {
                        ReportTemplateKey = message.Key,
                        message.SystemAccountKey,
                        message.Name,
                        ReportType = message.ReportType.ToString (),
                        Parameters = parms,
                        ReportStateCode = message.ReportState == null ? "NULL" : message.ReportState.CodedConcept.Code,
                        patientKey,
                        message.OrganizationKey,
                    } );
                connection.Execute (
                    "insert into AssessmentModule.Report values(@ReportKey, @SourceKey, @CreatedTimestamp, @Name, @NameFormat, @CanCustomize, @PatientKey, " +
                    "@ReportSeverity, @ReportType, @ReportStatus, @IsPatientViewable, @OrganizationKey)",
                    new
                    {
                        ReportKey = CombGuid.NewCombGuid (),
                        SourceKey = message.Key,
                        CreatedTimestamp = DateTime.Now,
                        message.Name,
                        NameFormat = "{0} " + parms,
                        CanCustomize = false,
                        patientKey,
                        ReportSeverity = string.Empty,
                        message.ReportType,
                        ReportStatus = "&nbsp;",
                        IsPatientViewable = false,
                        message.OrganizationKey
                    } );
            }
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public void Handle ( ReportTemplateNameChangedEvent message )
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public void Handle ( ReportTemplateReportTypeChangedEvent message )
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public void Handle ( ReportTemplateParametersChangedEvent message )
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public void Handle ( ReportTemplateReportStateChangedEvent message )
        {
            throw new NotImplementedException ();
        }

        /// <summary>
        ///     Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle ( RecentReportCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var parametersAssessmentScoreOverTime = message.Parameters as AssessmentScoreOverTimeParameters;

                const string CompleteQuery = @"SELECT [ReportKey], [SystemAccountKey], [RunDate] 
                                             FROM [OrganizationModule].[RecentReports] 
                                             WHERE SystemAccountKey=@SystemAccountKey 
                                             ORDER BY [RunDate]";
                const int MaxRows = 5;
                var recentReportsDtos = connection.Query<RecentReportsDto> ( CompleteQuery, new { message.SystemAccountKey } ).ToList ();
                if ( recentReportsDtos.Count () >= MaxRows )
                {
                    for ( var iRow = 0; iRow <= recentReportsDtos.Count () - MaxRows; iRow++ )
                    {
                        var reportKey = recentReportsDtos.ElementAt ( iRow ).ReportKey;
                        connection.Execute ( "DELETE FROM [OrganizationModule].[RecentReports] WHERE ReportKey='" + reportKey + "'" );
                    }
                }
                Guid? patientKey = null;
                if (parametersAssessmentScoreOverTime != null)
                {
                    patientKey = parametersAssessmentScoreOverTime.PatientKey;
                }
                connection.Execute (
                    "INSERT INTO OrganizationModule.RecentReports " +
                    "VALUES(@ReportKey, @SystemAccountKey, @Name, @Assessment, @RunDate, @PatientKey, @OrganizationKey)",
                    new
                        {
                            ReportKey = message.Key,
                            message.SystemAccountKey,
                            message.Name,
                            message.Assessment,
                            message.RunDate,
                            patientKey,
                            message.OrganizationKey,
                        } );
            }
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( ReportDefinitionCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    @"INSERT INTO ReportModule.ReportDefinition(ReportDefinitionKey, ReportName, DisplayName, IsPatientCentric) 
                                     VALUES(@ReportDefinitionKey, @ReportName, @DisplayName, @IsPatientCentric)",
                    new
                        {
                            ReportDefinitionKey = message.Key,
                            message.ReportName,
                            message.DisplayName,
                            message.IsPatientCentric
                        } );
            }
        }

        #endregion
    }
}