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
namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System;
    using System.Data;
    using Dapper;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;

    #endregion

    public class AssessmentInstanceUpdater : IHandleMessages<AssessmentCreatedEvent>,
                                             IHandleMessages<ItemUpdatedEvent>,
                                             IHandleMessages<AssessmentSubmittedEvent>,
                                             IHandleMessages<PercentCompleteUpdatedEvent>,
                                             IHandleMessages<AssessmentCanBeSelfAdministeredEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        public AssessmentInstanceUpdater(IDbConnectionFactory connectionFactory, IAssessmentInstanceRepository assessmentInstanceRepository)
        {
            _connectionFactory = connectionFactory;
            _assessmentInstanceRepository = assessmentInstanceRepository;
        }

        public void Handle(AssessmentCreatedEvent message)
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate ( message.Key );
            var createTime = DateTime.Now;
            const string cmd =
                @"INSERT INTO AssessmentModule.AssessmentInstance (AssessmentInstanceKey, AssessmentName, AssessmentCode, OrganizationKey, PatientKey, PercentComplete, CreatedTime, LastModifiedTime, IsSubmitted) 
                SELECT @AssessmentInstanceKey, AssessmentName, AssessmentCode, (SELECT OrganizationKey FROM PatientModule.Patient WHERE PatientKey = @PatientKey), 
                    @PatientKey, @PercentComplete, @CreatedTime, @LastModifiedTime, @IsSubmitted 
                FROM AssessmentModule.AssessmentDefinition WHERE AssessmentDefinitionKey =  @AssessmentDefinitionKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                    {
                        AssessmentInstanceKey = message.Key,
                        message.PatientKey,
                        PercentComplete = 0,
                        CreatedTime = createTime,
                        LastModifiedTime = lastModified ?? createTime,
                        IsSubmitted = false,
                        message.AssessmentDefinitionKey
                    });
            }
        }

        public void Handle(AssessmentSubmittedEvent message)
        {
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET IsSubmitted = @IsSubmitted
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                    {
                        AssessmentInstanceKey = message.Key,
                        IsSubmitted = message.Submit,
                    });
            }
        }

        public void Handle(AssessmentCanBeSelfAdministeredEvent message)
        {
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET CanSelfAdminister = @CanSelfAdminister
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                {
                    AssessmentInstanceKey = message.Key,
                    CanSelfAdminister = true
                });
            }
        }

        public void Handle(ItemUpdatedEvent message)
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate(message.Key);
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET LastModifiedTime = @LastModifiedTime
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                {
                    AssessmentInstanceKey = message.Key,
                    LastModifiedTime = lastModified,
                });
            }
        }

        public void Handle(PercentCompleteUpdatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute("UPDATE AssessmentModule.AssessmentInstance SET PercentComplete = @PercentComplete WHERE AssessmentInstanceKey = @AssessmentInstanceKey",
                                   new {PercentComplete = message.PercentComplete, AssessmentInstanceKey = message.Key});
            }
        }
    }
}