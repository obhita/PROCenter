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

namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Web.Http;

    using Dapper;

    using ProCenter.Common;
    using ProCenter.Mvc.Infrastructure;
    using ProCenter.Mvc.Models;
    using ProCenter.Service.Message.Assessment;

    #endregion

    /// <summary>The organization controller class.</summary>
    public class OrganizationController : BaseApiController
    {
        #region Constants

        private const string QueryActive = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.OrganizationAssessmentDefinition
                                 WHERE OrganizationKey=@OrganizationKey{0}
                             SELECT [t].AssessmentName,
                                    [t].AssessmentDefinitionKey AS 'Key',
                                    [t].AssessmentCode,
                                    [t].ScoreType                                         
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].AssessmentName) AS [ROW_NUMBER],   
                                             [t1].AssessmentName,
                                             [t1].AssessmentDefinitionKey,
                                             [t1].AssessmentCode,
                                             [t1].ScoreType    
                                 FROM OrganizationModule.OrganizationAssessmentDefinition AS [t1]
                                 WHERE OrganizationKey=@OrganizationKey{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

        private const string WhereConstraintActive = " AND (AssessmentName LIKE @search+'%')";

        #endregion

        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public OrganizationController ( IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager )
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Finders the search.</summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="search">The search.</param>
        /// <returns>A <see cref="FinderResults{AssessmentDefinitionDto}"/>.</returns>
        [HttpGet]
        public FinderResults<AssessmentDefinitionDto> FinderSearch ( int page, int pageSize, string search = null )
        {
            const string WhereConstraint = " AND ([t1].AssessmentName LIKE @search+'%')";
            const string Query = @"
                             SELECT COUNT(*) as TotalCount 
                             FROM AssessmentModule.AssessmentDefinition [t1] LEFT JOIN OrganizationModule.OrganizationAssessmentDefinition [t2] 
                                 ON [t1].AssessmentDefinitionKey = [t2].AssessmentDefinitionKey AND OrganizationKey=@OrganizationKey 
                                 WHERE [t2].AssessmentDefinitionKey IS NULL{0}
                             SELECT [t].AssessmentName,
                                    [t].AssessmentDefinitionKey AS 'Key',
                                    [t].AssessmentCode                                     
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].AssessmentName) AS [ROW_NUMBER],   
                                             [t1].AssessmentName,
                                             [t1].AssessmentDefinitionKey,
                                             [t1].AssessmentCode
                                 FROM AssessmentModule.AssessmentDefinition [t1] LEFT JOIN OrganizationModule.OrganizationAssessmentDefinition [t2] 
                                 ON [t1].AssessmentDefinitionKey = [t2].AssessmentDefinitionKey AND OrganizationKey=@OrganizationKey 
                                 WHERE [t2].AssessmentDefinitionKey IS NULL{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";
            var start = page * pageSize;
            var end = start + pageSize;
            var replaceString = string.IsNullOrWhiteSpace ( search ) ? string.Empty : WhereConstraint;
            var completeQuery = string.Format ( Query, replaceString );

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, search, UserContext.Current.OrganizationKey } ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto> ().ToList ();
                foreach ( var assessmentDefinitionDto in assessmentDefinitionDtos )
                {
                    assessmentDefinitionDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName ( assessmentDefinitionDto.AssessmentName )
                            .GetString ( SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode );
                }

                var findResults = new FinderResults<AssessmentDefinitionDto>
                                  {
                                      Data = assessmentDefinitionDtos,
                                      TotalCount = totalCount
                                  };

                return findResults;
            }
        }

        /// <summary>Finders the search active.</summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="search">The search.</param>
        /// <returns>A <see cref="FinderResults{AssessmentDefinitionDto}"/>.</returns>
        [HttpGet]
        public FinderResults<AssessmentDefinitionDto> FinderSearchActive ( int page, int pageSize, string search = null )
        {
            var start = page * pageSize;
            var end = start + pageSize;

            // replace all spaces with empty.string because the assessment names are stored without spaces in their names
            var replaceString = string.IsNullOrWhiteSpace ( search ) ? string.Empty : WhereConstraintActive;
            if ( search != null )
            {
                search = search.Replace(" ", string.Empty);
            }
            var completeQuery = string.Format(QueryActive, replaceString);

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, search, UserContext.Current.OrganizationKey } ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto> ().ToList ();
                foreach ( var assessmentDefinitionDto in assessmentDefinitionDtos )
                {
                    assessmentDefinitionDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName ( assessmentDefinitionDto.AssessmentName )
                            .GetString ( SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode );
                }

                var findResults = new FinderResults<AssessmentDefinitionDto>
                                  {
                                      Data = assessmentDefinitionDtos,
                                      TotalCount = totalCount
                                  };

                return findResults;
            }
        }

        /// <summary>Gets the specified s echo.</summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>A <see cref="DataTableResponse{AssessmentDefinitionDto}"/>.</returns>
        [HttpGet]
        public DataTableResponse<AssessmentDefinitionDto> Get ( string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null )
        {
            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraintActive;
            var completeQuery = string.Format ( QueryActive, replaceString );

            using ( var connection = _connectionFactory.CreateConnection () )
            {
                using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, search = sSearch, UserContext.Current.OrganizationKey } ) )
                {
                    var totalCount = multiQuery.Read<int> ().Single ();
                    var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto> ().ToList ();
                    foreach ( var assessmentDefinitionDto in assessmentDefinitionDtos )
                    {
                        assessmentDefinitionDto.AssessmentName =
                            _resourcesManager.GetResourceManagerByName ( assessmentDefinitionDto.AssessmentName )
                                .GetString ( SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode );
                    }
                    var dataTableResponse = new DataTableResponse<AssessmentDefinitionDto>
                                            {
                                                Data = assessmentDefinitionDtos,
                                                Echo = sEcho,
                                                TotalDisplayRecords = totalCount,
                                                TotalRecords = totalCount,
                                            };

                    return dataTableResponse;
                }
            }
        }

        #endregion
    }
}