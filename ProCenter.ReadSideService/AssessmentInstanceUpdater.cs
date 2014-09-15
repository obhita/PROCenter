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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Newtonsoft.Json;

    using NLog;

    using Pillar.Common.Utility;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Service.Message.Assessment;

    #endregion

    /// <summary>The assessment instance updater class.</summary>
    public class AssessmentInstanceUpdater : IHandleMessages<AssessmentCreatedEvent>,
                                             IHandleMessages<ItemUpdatedEvent>,
                                             IHandleMessages<AssessmentSubmittedEvent>,
                                             IHandleMessages<AssessmentCanBeSelfAdministeredEvent>,
                                             IHandleMessages<AssessmentScoredEvent>,
                                             IHandleMessages<UpdateEmailSentDateEvent>
    {
        #region Fields

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentInstanceUpdater" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public AssessmentInstanceUpdater (
            IDbConnectionFactory connectionFactory,
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IAssessmentDefinitionRepository assessmentDefinitionRepository ,
            IResourcesManager resourcesManager)
        {
            _connectionFactory = connectionFactory;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentCreatedEvent message )
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate ( message.Key );
            var createTime = DateTime.Now;
            const string Cmd =
                @"INSERT INTO AssessmentModule.AssessmentInstance 
                (AssessmentInstanceKey, AssessmentName, AssessmentCode, OrganizationKey, PatientKey, PercentComplete, CreatedTime, LastModifiedTime, IsSubmitted, CanSelfAdminister) 
                SELECT @AssessmentInstanceKey, AssessmentName, AssessmentCode, (SELECT OrganizationKey FROM PatientModule.Patient WHERE PatientKey = @PatientKey), 
                    @PatientKey, @PercentComplete, @CreatedTime, @LastModifiedTime, @IsSubmitted, @CanSelfAdminister 
                FROM AssessmentModule.AssessmentDefinition WHERE AssessmentDefinitionKey =  @AssessmentDefinitionKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    Cmd,
                    new
                        {
                            AssessmentInstanceKey = message.Key,
                            message.PatientKey,
                            PercentComplete = 0,
                            CreatedTime = createTime,
                            LastModifiedTime = lastModified ?? createTime,
                            IsSubmitted = false,
                            message.CanSelfAdminister,
                            message.AssessmentDefinitionKey,
                        } );
            }
        }

        /// <summary>Handles the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentSubmittedEvent message )
        {
            const string Cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET IsSubmitted = @IsSubmitted
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    Cmd,
                    new
                        {
                            AssessmentInstanceKey = message.Key,
                            IsSubmitted = message.Submit,
                        } );
            }
            InsertResponses ( message );
        }

        private void InsertResponses (AssessmentSubmittedEvent message)
        {
            var instance = _assessmentInstanceRepository.GetByKey(message.Key);
            const string Cmd =
                @"INSERT [AssessmentModule].[AssessmentInstanceResponse]
                    ([AssessmentInstanceKey]
                    ,[AssessmentName]
                    ,[AssessmentCode]
                    ,[AssessmentDefinitionKey]
                    ,[OrganizationKey]
                    ,[PatientKey]
                    ,[ItemDefinitionCode]
                    ,[ResponseType]
                    ,[ResponseValue]
                    ,[IsCode]
                    ,[CodeValue]) VALUES ( ";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var assessmentCode = string.Empty;
                var assessmentDefinitionKey = string.Empty;
                var sql = "SELECT AssessmentDefinitionKey AS [Key], AssessmentCode FROM [AssessmentModule].[AssessmentDefinition] WHERE [AssessmentDefinitionKey]='" 
                    + message.AssessmentDefinitionKey + "'";
                var rows = connection.Query<AssessmentDefinitionDto>(sql).ToList();
                if ( rows.Any () )
                {
                    assessmentCode = rows.ElementAt(0).AssessmentCode;
                    assessmentDefinitionKey = rows.ElementAt (0).Key.ToString();
                }
                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( _assessmentDefinitionRepository.GetKeyByCode ( assessmentCode ) );
                var items = assessmentDefinition.GetAllItemDefinitionsOfType(ItemType.Question).ToList();
                foreach ( var item in instance.ItemInstances )
                {
                    var isCode = false;
                    var val = item.Value;
                    int intVal = 0;
                    var responseType = GetTemplateName(items, item);
                    switch ( responseType )
                    {
                        case "LookupDto":
                            int.TryParse(((Lookup) item.Value).Value.ToString(), out intVal);
                            val = ( (Lookup)item.Value ).CodedConcept.Code;
                            responseType = item.Value.GetType ().Name;
                            isCode = true;
                            break;
                        case "MultipleSelect":
                            foreach ( var option in (IEnumerable<Lookup>)item.Value )
                            {
                                if (((IEnumerable<Lookup>)item.Value).Any())
                                {
                                    responseType = ((IEnumerable<Lookup>)item.Value).ElementAt(0).GetType().Name;
                                    int.TryParse(option.Value.ToString(), out intVal);
                                }
                                var fullQueryOption = Cmd
                                    + "'" + message.Key + "',"
                                    + "'" + instance.AssessmentName + "',"
                                    + "'" + assessmentCode + "',"
                                    + "'" + assessmentDefinitionKey + "',"
                                    + "'" + message.OrganizationKey + "',"
                                    + "'" + instance.PatientKey + "',"
                                    + "'" + item.ItemDefinitionCode + "'," +
                                    "'" + responseType + "',"
                                    + "'" + option.CodedConcept.Code + "',"
                                    + "'" + true + "',"
                                    + intVal + ")";
                                _logger.Info("InsertResponses Query: {0}", fullQueryOption);
                                connection.Execute(fullQueryOption);
                            }
                            continue;
                    }
                    var fullQuery = Cmd 
                                    + "'" + message.Key + "'," 
                                    + "'" + instance.AssessmentName + "'," 
                                    + "'" + assessmentCode + "',"
                                    + "'" + assessmentDefinitionKey + "',"
                                    + "'" + message.OrganizationKey + "'," 
                                    + "'" + instance.PatientKey + "',"
                                    + "'" + item.ItemDefinitionCode + "'," +
                                    "'" + responseType + "'," 
                                    + "'" + val + "',"
                                    + "'" + isCode + "',"
                                    + intVal + ")"; 
                    _logger.Info("InsertResponses Query: {0}", fullQuery);
                    connection.Execute ( fullQuery );
                }
            }
        }

        private string GetTemplateName (IEnumerable<ItemDefinition> itemDefintions, ItemInstance item)
        {
            var item1 = item;
            foreach ( var metaDataItem in itemDefintions.Where
                (itemInstance => itemInstance.CodedConcept.Code == item1.ItemDefinitionCode).
                SelectMany ( itemInstance => itemInstance.ItemMetadata.MetadataItems.OfType<ItemTemplateMetadataItem>() ) )
            {
                return metaDataItem.TemplateName;
            }
            return string.Empty;
        }

        /// <summary>Handles the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentCanBeSelfAdministeredEvent message )
        {
            const string Cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET CanSelfAdminister = @CanSelfAdminister
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    Cmd,
                    new
                        {
                            AssessmentInstanceKey = message.Key,
                            CanSelfAdminister = true
                        } );
            }
        }

        /// <summary>Handles the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Handle ( ItemUpdatedEvent message )
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate ( message.Key );
            var assessmentInstance = _assessmentInstanceRepository.GetByKey ( message.Key );
            const string Cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET LastModifiedTime = @LastModifiedTime
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    Cmd,
                    new
                        {
                            AssessmentInstanceKey = message.Key,
                            LastModifiedTime = lastModified,
                        } );
                connection.Execute (
                    "UPDATE AssessmentModule.AssessmentInstance SET PercentComplete = @PercentComplete WHERE AssessmentInstanceKey = @AssessmentInstanceKey",
                    new { assessmentInstance.CalculateCompleteness ().PercentComplete, AssessmentInstanceKey = assessmentInstance.Key } );
            }
        }

        /// <summary>Handles the specified message.</summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentScoredEvent message )
        {
            var assessment = _assessmentInstanceRepository.GetByKey ( message.Key );
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessment.AssessmentDefinitionKey );
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var assessmentScoreString = message.Value.GetType ().IsValueType ? message.Value.ToString () : JsonConvert.SerializeObject ( message.Value );
                connection.Execute (
                    "INSERT INTO AssessmentModule.AssessmentScores VALUES (@AssessmentScoresKey, @AssessmentDefinitionCode, @AssessmentScore, @ScoredDate, @PatientKey)",
                    new
                        {
                            AssessmentScoresKey = CombGuid.NewCombGuid (),
                            AssessmentDefinitionCode = assessmentDefinition.CodedConcept.Code,
                            AssessmentScore = assessmentScoreString,
                            ScoredDate = DateTime.Now,
                            assessment.PatientKey
                        } );
            }
            if ( assessment.Score.HasReport )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                        "insert into AssessmentModule.Report values(@ReportKey, @SourceKey, @CreatedTimestamp, @Name, @NameFormat, @CanCustomize, @PatientKey, " +
                        "@ReportSeverity, @ReportType, @ReportStatus, @IsPatientViewable, @OrganizationKey)",
                        new
                            {
                                ReportKey = CombGuid.NewCombGuid (),
                                SourceKey = assessment.Key,
                                CreatedTimestamp = DateTime.Now,
                                Name = assessment.AssessmentName,
                                NameFormat = "{0}",
                                CanCustomize = false,
                                assessment.PatientKey,
                                ReportSeverity = ( assessment.Score.Value is IGenerateReport ) ? ( assessment.Score.Value as IGenerateReport ).Severity : ReportSeverity.Unknown,
                                ReportType = ReportType.Canned,
                                ReportStatus = "&nbsp;",
                                IsPatientViewable = false,
                                message.OrganizationKey
                            } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( UpdateEmailSentDateEvent message )
        {
            const string Cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET EmailSentDate = @EmailSentDate, EmailFailedDate = @EmailFailedDate
                    where AssessmentInstanceKey = @AssessmentInstanceKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    Cmd,
                    new
                        {
                            AssessmentInstanceKey = message.Key,
                            message.EmailSentDate,
                            message.EmailFailedDate
                        } );
            }
        }

        #endregion
    }
}