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

namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using System;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.OrganizationModule;
    using Domain.SecurityModule;
    using global::AutoMapper;
    using ProCenter.Common;
    using Service.Message.Organization;
    using Service.Message.Security;

    #endregion

    /// <summary>The get staff dto by key request handler class.</summary>
    public class GetStaffDtoByKeyRequestHandler : ServiceRequestHandler<GetStaffDtoByKeyRequest, GetStaffDtoResponse>
    {
        #region Fields

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IStaffRepository _staffRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStaffDtoByKeyRequestHandler"/> class.
        /// </summary>
        /// <param name="staffRepository">The staff repository.</param>
        /// <param name="systemAccountRepository">The system account repository.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public GetStaffDtoByKeyRequestHandler ( IStaffRepository staffRepository, ISystemAccountRepository systemAccountRepository, IDbConnectionFactory dbConnectionFactory )
        {
            _staffRepository = staffRepository;
            _systemAccountRepository = systemAccountRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetStaffDtoByKeyRequest request, GetStaffDtoResponse response )
        {
            var staff = _staffRepository.GetByKey ( request.Key );
            var staffDto = Mapper.Map<Staff, StaffDto> ( staff );

            //get system account associated with staff
            Guid? systemAccountKey;
            using ( var connection = _dbConnectionFactory.CreateConnection () )
            {
                systemAccountKey =
                    connection.Query<Guid?> ( "SELECT SystemAccountKey FROM SecurityModule.SystemAccount WHERE StaffKey=@StaffKey", new {StaffKey = request.Key} )
                    .FirstOrDefault ();
            }
            if ( systemAccountKey.HasValue )
            {
                var systemAccount = _systemAccountRepository.GetByKey ( systemAccountKey.Value );
                var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto> ( systemAccount );
                if ( systemAccount.RoleKeys.Any () )
                {
                    var roleKeys = string.Join ( ", ", systemAccount.RoleKeys );
                    roleKeys = "'" + roleKeys.Replace ( ", ", "', '" ) + "'";
                    var query = string.Format ( "SELECT RoleKey as 'Key', Name FROM SecurityModule.Role WHERE RoleKey IN ({0})", roleKeys );
                    using ( var connection = _dbConnectionFactory.CreateConnection () )
                    {
                        var roleDtos = connection.Query<RoleDto> ( query ).OrderBy ( r => r.Name );
                        systemAccountDto.Roles = roleDtos;
                    }
                }

                staffDto.SystemAccount = systemAccountDto;
            }
            response.DataTransferObject = staffDto;
        }

        #endregion
    }
}