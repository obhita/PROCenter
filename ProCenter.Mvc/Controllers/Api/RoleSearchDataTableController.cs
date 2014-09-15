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
    using Models;
    using Service.Message.Security;

    #endregion

    /// <summary>The role search data table controller class.</summary>
    public class RoleSearchDataTableController : BaseApiController
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleSearchDataTableController"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public RoleSearchDataTableController ( IDbConnectionFactory connectionFactory )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the specified s echo.</summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>A <see cref="DataTableResponse{RoleDto}"/>.</returns>
        public DataTableResponse<RoleDto> Get ( string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null )
        {
            const string WhereConstraint = " AND (Name LIKE @Search+'%')";
            const string Query = @"
                            SELECT COUNT(*) as TotalCount FROM SecurityModule.Role
                                WHERE OrganizationKey='{0}'{1}
                            SELECT [t].Name,
                                   [t].RoleKey AS 'Key'
                            FROM (
                                SELECT ROW_NUMBER() OVER (
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],
                                             [T1].Name,   
                                             [t1].RoleKey
                                FROM SecurityModule.Role AS [t1]
                                WHERE OrganizationKey='{0}'{1}   
                                ) AS [t]
                            WHERE [t].[ROW_NUMBER] BETWEEN @start +1 AND @end
                            ORDER BY [t].[ROW_NUMBER]";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraint;
            var completeQuery = string.Format ( Query, UserContext.Current.OrganizationKey, replaceString );
            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery, new {start, end, search = sSearch} ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var roleDtos = multiQuery.Read<RoleDto> ();
                var dataTableResponse = new DataTableResponse<RoleDto>
                {
                    Data = roleDtos.ToList (),
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