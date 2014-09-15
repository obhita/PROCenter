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

    using System.Linq;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using Service.Message.Assessment;

    #endregion

    /// <summary>All assessments data table controller class.</summary>
    public class AllAssessmentsDataTableController : BaseApiController
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllAssessmentsDataTableController"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public AllAssessmentsDataTableController ( IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager )
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the specified s echo.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>A <see cref="DataTableResponse{AssessmentSummaryDto}"/>.</returns>
        public DataTableResponse<AssessmentSummaryDto> Get ( string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null )
        {
            const string WhereConstraint = "WHERE [p1].OrganizationKey = @OrganizationKey AND ( [p1].FirstName LIKE @search+'%' OR [p1].LastName LIKE @search+'%')";
            const string Query = @"
                             SELECT COUNT(*) as TotalCount FROM AssessmentModule.AssessmentInstance
                             SELECT [t].*                                    
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].CreatedTime DESC) AS [ROW_NUMBER],   
                                             [t1].AssessmentInstanceKey,
                                             [t1].AssessmentName, 
                                             [t1].AssessmentCode,
                                             [t1].PercentComplete,  
                                             [t1].CreatedTime,  
                                             [t1].IsSubmitted,  
                                             [t1].PatientKey,
                                             [p1].FirstName AS PatientFirstName,
                                             [p1].LastName AS PatientLastName
                                 FROM AssessmentModule.AssessmentInstance AS [t1]
                                 JOIN PatientModule.Patient AS [p1]
                                 ON t1.PatientKey=p1.PatientKey
                                 {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraint;
            var completeQuery = string.Format ( Query, replaceString );

            using ( var conn = _connectionFactory.CreateConnection () )
            using ( var multiQuery = conn.QueryMultiple ( completeQuery, new {start, end, search = sSearch, UserContext.Current.OrganizationKey} ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var assessmentSummaryDtos = multiQuery.Read<AssessmentSummaryDto> ().ToList ();
                foreach ( var assessmentSummaryDto in assessmentSummaryDtos )
                {
                    assessmentSummaryDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName ( assessmentSummaryDto.AssessmentName )
                            .GetString ( SharedStringNames.ResourceKeyPrefix + assessmentSummaryDto.AssessmentCode );
                }
                var dataTableResponse = new DataTableResponse<AssessmentSummaryDto>
                {
                    Data = assessmentSummaryDtos,
                    Echo = sEcho,
                    TotalDisplayRecords = totalCount,
                    TotalRecords = totalCount,
                };

                return dataTableResponse;
            }
        }

        #endregion
    }
}