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

namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;
    using Agatha.Common;
    using Dapper;
    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Mvc.Infrastructure;
    using ProCenter.Mvc.Models;
    using ProCenter.Mvc.Views.Shared;
    using ProCenter.Service.Message.Assessment;

    using Group = ProCenter.Service.Message.Assessment.Group;

    #endregion

    /// <summary>The assessment controller class.</summary>
    public class AssessmentController : BaseApiController
    {
        #region Constants

        private const string QueryActive = @"SELECT 
                                           [AssessmentName]
                                           ,[AssessmentCode]
                                           ,[OrganizationKey]
                                           FROM [OrganizationModule].[OrganizationAssessmentDefinition]
                                           WHERE OrganizationKey = '{0}' 
                                           AND AssessmentDefinitionKey = '{1}'";

        #endregion

        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public AssessmentController (
            IRequestDispatcherFactory requestDispatcherFactory,
            IDbConnectionFactory connectionFactory,
            IResourcesManager resourcesManager,
            IAssessmentDefinitionRepository assessmentDefinitionRepository )
            : base ( requestDispatcherFactory )
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the report data table.</summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>A <see cref="DataTableResponse{ReportSummaryDto}" />.</returns>
        public DataTableResponse<ReportSummaryDto> GetReportDataTable ( string sEcho, int iDisplayStart, int iDisplayLength, Guid? patientKey = null, string sSearch = null )
        {
            const string WhereConstraint = "PatientKey = @patientKey";
            const string WhereSearchConstraint = "(Name LIKE @search+'%')";
            const string WherePatientViewableConstraint = "IsPatientViewable = 1";
            const string Query = @"
                             SELECT COUNT(*) as TotalCount FROM AssessmentModule.Report
                                 WHERE ReportType = 0 AND {0}
                             SELECT [t].SourceKey,
                                    [t].CreatedTimestamp as 'CreatedTime', 
                                    [t].Name,  
                                    [t].NameFormat,  
                                    [t].CanCustomize,
                                    [t].PatientKey,   
                                    [t].ReportSeverity, 
                                    [t].ReportStatus,  
                                    [t].ReportType, 
                                    [t].ReportKey as 'Key' ,
                                    [t].OrganizationKey  
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].Name,
                                             [t1].NameFormat,
                                             [t1].CreatedTimestamp, 
                                             [t1].CanCustomize,  
                                             [t1].PatientKey  ,
                                             [t1].ReportSeverity, 
                                             [t1].ReportStatus,  
                                             [t1].ReportType,    
                                             [t1].SourceKey  ,   
                                             [t1].ReportKey ,
                                             [t1].OrganizationKey  
                                 FROM AssessmentModule.Report AS [t1]
                                 WHERE ReportType = 0 AND {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[CreatedTimestamp] DESC";

            if ( UserContext.Current.PatientKey.HasValue && UserContext.Current.PatientKey != patientKey )
            {
                return new DataTableResponse<ReportSummaryDto>
                       {
                           Data = Enumerable.Empty<ReportSummaryDto> (),
                           Echo = sEcho,
                           TotalDisplayRecords = 0,
                           TotalRecords = 0
                       };
            }

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var whereConstraintBuilder = new StringBuilder ( "OrganizationKey = @OrganizationKey " );
            if ( patientKey.HasValue || sSearch != null || UserContext.Current.PatientKey.HasValue )
            {
                if ( patientKey.HasValue )
                {
                    whereConstraintBuilder.Append ( " AND " + WhereConstraint );
                }
                if ( sSearch != null )
                {
                    whereConstraintBuilder.Append ( " AND " + WhereSearchConstraint );
                }
                if ( UserContext.Current.PatientKey.HasValue )
                {
                    whereConstraintBuilder.Append ( " AND " + WherePatientViewableConstraint );
                }
            }
            var completeQuery = string.Format ( Query, whereConstraintBuilder );
            var totalCount = 0;
            IEnumerable<ReportSummaryDto> reportDtos = null;
            try
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, patientKey, search = sSearch, UserContext.Current.OrganizationKey } ) )
                {
                    totalCount = multiQuery.Read<int> ().Single ();
                    reportDtos = multiQuery.Read<ReportSummaryDto> ().ToList ();

                    foreach ( var dto in reportDtos )
                    {
                        dto.DisplayName = string.Format ( dto.NameFormat, _resourcesManager.GetResourceManagerByName ( dto.Name ).GetString ( SharedStringNames.ReportName ) );
                    }
                }
            }
            catch ( Exception )
            {
            }

            return new DataTableResponse<ReportSummaryDto>
                   {
                       Data = reportDtos,
                       Echo = sEcho,
                       TotalDisplayRecords = totalCount,
                       TotalRecords = totalCount,
                   };
        }

        /// <summary>Gets the type of the reports by.</summary>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="type">The type.</param>
        /// <param name="rowCount">The row count.</param>
        /// <returns>A <see cref="IEnumerable{ReportSummaryDto}" />.</returns>
        public async Task<IEnumerable<ReportSummaryDto>> GetReportsByType ( Guid patientKey, int type, int rowCount )
        {
            const string Query = @"SELECT TOP {0} 
                                          [t].SourceKey,
                                          [t].CreatedTimestamp as 'CreatedTime', 
                                          [t].Name,  
                                          [t].NameFormat,
                                          [t].CanCustomize,
                                          [t].PatientKey,   
                                          [t].ReportSeverity, 
                                          [t].ReportStatus,  
                                          [t].ReportType,
                                          [t].ReportKey as 'Key' ,
                                          [t].OrganizationKey    
                                 FROM AssessmentModule.Report AS [t]
                                 WHERE [t].PatientKey = @patientKey AND [t].ReportType = @type
                                 ORDER BY [t].CreatedTimestamp DESC";

            if ( UserContext.Current.PatientKey.HasValue && UserContext.Current.PatientKey != patientKey )
            {
                return Enumerable.Empty<ReportSummaryDto> ();
            }

            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var reportDtos = await connection.QueryAsync<ReportSummaryDto> ( string.Format ( Query, rowCount ), new { patientKey, type } );

                foreach ( var dto in reportDtos )
                {
                    dto.DisplayName = string.Format ( dto.NameFormat, _resourcesManager.GetResourceManagerByName ( dto.Name ).GetString ( SharedStringNames.ReportName ) );
                }
                return reportDtos.ToList ();
            }
        }

        /// <summary>
        /// Lookups the search active.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="search">The search.</param>
        /// <returns>
        /// Returns FinderResults as type IItem.
        /// </returns>
        public FinderResults<IItem> GetLookupSearchActive ( int page, int pageSize, string assessmentDefinitionKey, string search = null )
        {
            var questionsGroups = new List<IItem> ();
            var totalCount = 0;
            var assessmentDefinitionDto = GetAssessmentDefinitionForResponse(assessmentDefinitionKey);
            if ( assessmentDefinitionDto != null )
            {
                var assessmentResourceManager = _resourcesManager.GetResourceManagerByName(assessmentDefinitionDto.AssessmentName);
                var assessmentName = assessmentResourceManager.GetString(SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode);
                var instance = _assessmentDefinitionRepository.GetByKey ( _assessmentDefinitionRepository.GetKeyByCode ( assessmentDefinitionDto.AssessmentCode ) );
                foreach ( var itemDef in instance.ItemDefinitions )
                {
                    var str = assessmentResourceManager
                        .GetString ( SharedStringNames.ResourceKeyPrefix + itemDef.CodedConcept.Code );
                    if ( str == null )
                    {
                        str = assessmentResourceManager
                            .GetString(SharedStringNames.ResourceKeyPrefix + itemDef.CodedConcept.Code + SharedStringNames.ResourceKeyPrefix + SharedStringNames.Instructions);
                        if ( str == null )
                        {
                            continue;
                        }
                    }
                    if (itemDef.ItemType == ItemType.Question && IsMatch ( search, str ))
                    {
                        var q = new Question 
                        { 
                            ItemType = ItemType.Question.DisplayName, 
                            Code = itemDef.CodedConcept.Code, 
                            Name = str, 
                            TemplateName = GetTemplateName ( itemDef ), 
                            AssessmentName = assessmentName, 
                            AssessmentCode = assessmentDefinitionDto.AssessmentCode 
                        };
                        questionsGroups.Add ( q );
                        totalCount += 1;
                    }
                    var groups = GetGroups ( assessmentDefinitionDto, itemDef, search, assessmentName );
                    var sections = GetSections ( assessmentDefinitionDto, itemDef, search, assessmentName );

                    totalCount += sections.Select ( a => a.ItemType == ItemType.Question.DisplayName ).Count ()
                                  + groups.Select ( a => a.ItemType == ItemType.Question.DisplayName ).Count ();
                    questionsGroups.AddRange ( sections );
                    questionsGroups.AddRange ( groups );
                }

                questionsGroups = AssignLevels ( questionsGroups );
            }
            var findResults = new FinderResults<IItem>
                                {
                                    Data = questionsGroups,
                                    TotalCount = totalCount
                                };
            return findResults;
        }

        #endregion

        #region Methods

        private string GetTemplateName (ItemDefinition itemDefinition)
        {
            var metaData = itemDefinition.ItemMetadata.MetadataItems.FirstOrDefault(a => a.GetType() == typeof(ItemTemplateMetadataItem));
            var templateMetaData = metaData as ItemTemplateMetadataItem;
            if ( templateMetaData == null )
            {
                return null;
            }
            var returnName = templateMetaData.TemplateName;
            switch (templateMetaData.TemplateName)
            {
                case "Int32":
                    returnName = "IntRange";
                    break;
                case "LookupDto":
                    returnName = "MultipleSelect";
                    break;
            }
            return returnName;
        }

        private AssessmentDefinitionDto GetAssessmentDefinitionForResponse ( string assessmentDefinitionKey )
        {
            var completeQuery = string.Format(QueryActive, UserContext.Current.OrganizationKey, assessmentDefinitionKey);

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery ) )
            {
                return multiQuery.Read<AssessmentDefinitionDto> ().FirstOrDefault();
            }
        }

        private List<IItem> AssignLevels ( List<IItem> questionGroups )
        {
            foreach ( var item in questionGroups )
            {
                var level = 1;
                item.Level = level;
                if ( item.ItemType != ItemType.Group.DisplayName )
                {
                    continue;
                }
                level += 1;
                var groups = ( (Group)item ).Items.Where ( a => a.ItemType == ItemType.Group ).ToList ();
                if ( groups.Any () )
                {
                    return AssignLevels ( groups );
                }
                var questions = ( (Group)item ).Items.Where ( a => a.ItemType == ItemType.Question );
                foreach ( var q in questions )
                {
                    ( (Question)q ).Level = level;
                }
            }
            return questionGroups;
        }

        private IItem GetGroup(AssessmentDefinitionDto assessmentDefinitionDto, ItemDefinition itemDefinition, List<IItem> questions, string assessmentName)
        {
            var groupCode = itemDefinition.CodedConcept.Code;
            var assessmentResourceManager = _resourcesManager.GetResourceManagerByName(assessmentDefinitionDto.AssessmentName);
            if (itemDefinition.ItemDefinitions != null)
            {
                var parentGroup = itemDefinition;
                foreach (var id1 in itemDefinition.ItemDefinitions
                    .Where(id1 => id1.ItemDefinitions != null)
                    .Where(id1 => id1.ItemDefinitions.Any(id2 => id2.CodedConcept.Code == questions.ElementAt(0).Code)))
                {
                    parentGroup = id1;
                }
                if (parentGroup != null)
                {
                    groupCode = parentGroup.CodedConcept.Code;
                }
            }
            var groupName = assessmentResourceManager
                .GetString(SharedStringNames.ResourceKeyPrefix + groupCode + SharedStringNames.ResourceKeyPrefix + SharedStringNames.Instructions)
                            ?? assessmentResourceManager.GetString(SharedStringNames.ResourceKeyPrefix + groupCode);

            var group = new Group
            {
                ItemType = ItemType.Group.DisplayName,
                Code = itemDefinition.CodedConcept.Code,
                Name = groupName,
                Items = questions,
                AssessmentName = assessmentName,
                AssessmentCode = assessmentDefinitionDto.AssessmentCode
            };
            return group;
        }

        private List<IItem> GetGroups ( AssessmentDefinitionDto assessmentDefinitionDto, ItemDefinition itemDefinition, string search, string assessmentName )
        {
            var groups = new List<IItem> ();
            var questions = new List<IItem> ();

            if ( itemDefinition.ItemType != ItemType.Group || itemDefinition.ItemDefinitions == null )
            {
                return groups;
            }
            var q = GetQuestions ( assessmentDefinitionDto, itemDefinition.ItemDefinitions, search, assessmentName );
            questions.AddRange ( q );
            if ( !q.Any () )
            {
                return groups;
            }
            var group = GetGroup ( assessmentDefinitionDto, itemDefinition, q, assessmentName );
            groups.Add (group );
            foreach ( var question in q )
            {
                ((Question)question ).ParentName = group.Name;
            }

            return groups;
        }

        private List<IItem> GetQuestions ( AssessmentDefinitionDto assessmentDefinitionDto, IEnumerable<ItemDefinition> itemDefinitions, string search, string assessmentName )
        {
            var definitions = itemDefinitions as ItemDefinition[] ?? itemDefinitions.ToArray ();
            var questions = definitions.Where ( a => a.ItemType == ItemType.Question ).ToList ();
            var subGroupQuestions = definitions
                .Where ( a => a.ItemType == ItemType.Group )
                .SelectMany ( b => b.ItemDefinitions )
                .Where ( c => c.ItemType == ItemType.Question );
            var subGroupInGroupQuestions = definitions
                .Where(a => a.ItemType == ItemType.Group)
                .Where( g => g.ItemType == ItemType.Group)
                .SelectMany(b => b.ItemDefinitions)
                .Where(c => c.ItemType == ItemType.Question);

            questions.AddRange(subGroupQuestions);
            questions.AddRange(subGroupInGroupQuestions);
            questions = questions.GroupBy ( a => a.CodedConcept.Code ).Select ( a => a.First () ).ToList ();
            return (from q in questions
                let questionName =
                    _resourcesManager.GetResourceManagerByName ( assessmentDefinitionDto.AssessmentName )
                        .GetString ( SharedStringNames.ResourceKeyPrefix + q.CodedConcept.Code )
                where IsMatch ( search, questionName )
                    select new Question 
                    { 
                        ItemType = ItemType.Question.DisplayName, 
                        Code = q.CodedConcept.Code, 
                        Name = questionName, 
                        TemplateName = GetTemplateName(q), 
                        AssessmentName = assessmentName, 
                        AssessmentCode = assessmentDefinitionDto.AssessmentCode
                    })
                .Cast<IItem>().OrderBy(a => a.Name).ToList();
        }

        private List<IItem> GetSections ( AssessmentDefinitionDto assessmentDefinitionDto, ItemDefinition itemDefinition, string search, string assessmentName )
        {
            var groups = new List<IItem>();
            var questions = new List<IItem>();

            if (itemDefinition.ItemType != ItemType.Section || itemDefinition.ItemDefinitions == null)
            {
                return groups;
            }
            var q = GetQuestions(assessmentDefinitionDto, itemDefinition.ItemDefinitions, search, assessmentName);
            questions.AddRange(q);
            if (q.Any())
            {
                var group = GetGroup(assessmentDefinitionDto, itemDefinition, q, assessmentName);
                groups.Add(group);
                foreach (var question in q)
                {
                    ((Question)question).ParentName = group.Name;
                }
            }
            return groups;
        }

        private bool IsMatch ( string search, string textToSearch )
        {
            return textToSearch == null || textToSearch.ToLower ().Contains ( search.ToLower () );
        }

        #endregion
    }
}