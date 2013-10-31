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
namespace ProCenter.Service.Handler.Patient
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.MessageModule;
    using Infrastructure.Service.ReadSideService;
    using ProCenter.Common;
    using Service.Message.Assessment;
    using Service.Message.Message;
    using Service.Message.Patient;

    #endregion

    public class GetPatientDashboardRequestHandler : ServiceRequestHandler<GetPatientDashboardRequest, GetPatientDashboardResponse>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetPatientDashboardRequestHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected override void Handle(GetPatientDashboardRequest request, GetPatientDashboardResponse response)
        {
            var forSelfAdministrationClause = "AND ForSelfAdministration = 1";
            var canBeSelfAdministeredClause = "AND CanSelfAdminister = 1";

            const string query = @"SELECT TOP 1 AssessmentInstanceKey AS 'Key', AssessmentName, AssessmentCode, PercentComplete, CreatedTime, LastModifiedTime, IsSubmitted, PatientKey
                        FROM AssessmentModule.AssessmentInstance
                        WHERE PatientKey = @PatientKey AND DATEADD(day, 7, CreatedTime) > GetDate() {2} ORDER BY CreatedTime DESC

                        SELECT WorkflowMessageKey AS 'Key', w.PatientKey, InitiatingAssessmentDefinitionKey AS 'InitiatingAssessmentKey', InitiatingAssessmentDefinitionCode AS 
                            'InitiatingAssessmentCode', RecommendedAssessmentDefinitionKey, RecommendedAssessmentDefinitionCode, RecommendedAssessmentDefinitionName, p.FirstName AS 
                            'PatientFirstName', p.LastName AS 'PatientLastName', w.InitiatingAssessmentScore AS 'ScoreValue'
                        FROM MessageModule.WorkflowMessage w JOIN PatientModule.Patient p ON w.PatientKey = p.PatientKey
                        WHERE w.PatientKey = @PatientKey AND WorkflowMessageStatus = '{0}' AND DATEADD(day, 7, CreatedDate) > GetDate() {3} ORDER BY CreatedDate DESC 

                        SELECT AssessmentReminderKey AS 'Key', PatientKey, AssessmentDefinitionKey, AssessmentName, AssessmentCode, Title, Start                
                                 FROM MessageModule.AssessmentReminder 
                                 WHERE PatientKey = @PatientKey AND Status = '{1}' AND GetDate() >= DATEADD(day, -ReminderDays,Start) {3}

                        SELECT COUNT(*) as Total FROM AssessmentModule.AssessmentInstance WHERE PatientKey = @PatientKey {2}

                        SELECT Min(CreatedTime) FROM AssessmentModule.AssessmentInstance WHERE PatientKey = @PatientKey {2}";
            if ( UserContext.Current.PatientKey == null )
            {
                forSelfAdministrationClause = canBeSelfAdministeredClause = string.Empty;
            }
            var completeQuery = string.Format(query, WorkflowMessageStatus.WaitingForResponse, AssessmentReminderStatus.Default, canBeSelfAdministeredClause, forSelfAdministrationClause);

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var multiQuery = connection.QueryMultiple(completeQuery, new {request.PatientKey}))
                {
                    var assessmentSummaryDtos = multiQuery.Read<AssessmentSummaryDto>().ToList();

                    var workflowMessageDtos = multiQuery.Read<WorkflowMessageDto, string, WorkflowMessageDto>((workflowMessageDto, scoreValue) =>
                        {
                            workflowMessageDto.InitiatingAssessmentScore = new ScoreDto {Value = scoreValue};
                            return workflowMessageDto;
                        }, "scoreValue").ToList();

                    var assessmentReminderDtos = multiQuery.Read<AssessmentReminderDto> ().ToList ();

                    var totalCount = multiQuery.Read<int>().Single();
                    DateTime? startingDate = null;
                    if (totalCount != 0)
                    {
                        startingDate = multiQuery.Read<DateTime>().Single();
                    }

                    var dashboardItems = new List<object>();
                    dashboardItems.AddRange(assessmentSummaryDtos);
                    dashboardItems.AddRange(workflowMessageDtos);
                    dashboardItems.AddRange ( assessmentReminderDtos );
                    dashboardItems.Add(new TotalAssessmentsDto {Total = totalCount, StartingDate = startingDate});

                    response.DashboardItems = dashboardItems;
                }
            }
        }
    }
}