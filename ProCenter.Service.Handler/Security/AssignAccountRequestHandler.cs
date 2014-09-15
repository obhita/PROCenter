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

namespace ProCenter.Service.Handler.Security
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.SecurityModule;
    using global::AutoMapper;
    using global::AutoMapper.Mappers;

    using Infrastructure.Service;

    using Newtonsoft.Json;

    using NLog;
    using Pillar.Domain.Primitives;
    using ProCenter.Common;
    using Service.Message.Common;
    using Service.Message.Security;

    #endregion

    /// <summary>Handler for assigning account to staff/patient.</summary>
    public class AssignAccountRequestHandler : ServiceRequestHandler<AssignAccountRequest, AssignAccountResponse>
    {
        #region Fields

        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ISystemAccountIdentityServiceManager _systemAccountIdentityServiceManager;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssignAccountRequestHandler" /> class.
        /// </summary>
        /// <param name="systemAccountRepository">The system account repository.</param>
        /// <param name="dbConnectionFactory">The db connection factory.</param>
        /// <param name="systemAccountIdentityServiceManager">The system account identity service manager.</param>
        public AssignAccountRequestHandler ( ISystemAccountRepository systemAccountRepository,
            IDbConnectionFactory dbConnectionFactory,
            ISystemAccountIdentityServiceManager systemAccountIdentityServiceManager )
        {
            _systemAccountRepository = systemAccountRepository;
            _dbConnectionFactory = dbConnectionFactory;
            _systemAccountIdentityServiceManager = systemAccountIdentityServiceManager;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <exception cref="System.InvalidOperationException">Cannot find Patient portal built in role.</exception>
        protected override void Handle ( AssignAccountRequest request, AssignAccountResponse response )
        {
            if ( request.SystemAccountDto.CreateNew )
            {
                var systemAccount = _systemAccountRepository.GetByIdentifier ( request.SystemAccountDto.Identifier );
                if ( systemAccount != null )
                {
                    // account existing
                    var dataErrorInfo =
                        new DataErrorInfo ( string.Format ( "Cannot create account because an account with the email {0} already exists.", request.SystemAccountDto.Identifier ),
                            ErrorLevel.Error );
                    response.SystemAccountDto = request.SystemAccountDto;
                    response.SystemAccountDto.AddDataErrorInfo ( dataErrorInfo );
                }
                else
                {
                    var identityServiceResponse = _systemAccountIdentityServiceManager.Create ( request.SystemAccountDto.Email );
                    if ( identityServiceResponse.Sucess )
                    {
                        var systemAccountFactory = new SystemAccountFactory ();
                        systemAccount = systemAccountFactory.Create ( request.OrganizationKey, request.SystemAccountDto.Email, new Email ( request.SystemAccountDto.Email ) );
                        if ( request.StaffKey != Guid.Empty )
                        {
                            systemAccount.AssignToStaff ( request.StaffKey );
                        }
                        if ( request.PatientKey != Guid.Empty )
                        {
                            systemAccount.AssignToPatient ( request.PatientKey );

                            Guid? portalRoleKey;
                            using ( var connection = _dbConnectionFactory.CreateConnection () )
                            {
                                portalRoleKey =
                                    connection.Query<Guid?> ( "SELECT [RoleKey] FROM [SecurityModule].[Role] WHERE Name=@Name", new {Name = "Patient Portal"} ).FirstOrDefault ();
                            }
                            if ( portalRoleKey.HasValue )
                            {
                                systemAccount.AddRole ( portalRoleKey.Value );
                            }
                            else
                            {
                                throw new InvalidOperationException ( "Cannot find Patient portal built in role." );
                            }
                        }
                        var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto> ( systemAccount );
                        response.SystemAccountDto = systemAccountDto;
                    }
                    else
                    {
                        var result = identityServiceResponse.ErrorMessage;
                        //// remove the message from the JSON
                        var identityError = (IdentityServerError)JsonConvert.DeserializeObject ( result, typeof(IdentityServerError));
                        var dataErrorInfo = new DataErrorInfo(identityError.Message, ErrorLevel.Error);
                        response.SystemAccountDto = request.SystemAccountDto;
                        response.SystemAccountDto.AddDataErrorInfo ( dataErrorInfo );
                    }
                }
            }
            else
            {
                var systemAccount = _systemAccountRepository.GetByIdentifier ( request.SystemAccountDto.Identifier );
                if ( systemAccount != null ) 
                {
                    // account existing
                    if ( systemAccount.StaffKey == null )
                    {
                        systemAccount.AssignToStaff ( request.StaffKey );
                        var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto> ( systemAccount );
                        response.SystemAccountDto = systemAccountDto;
                    }
                    else
                    {
                        var dataErrorInfo =
                            new DataErrorInfo (
                                string.Format ( 
                                "Cannot link account because an account with the email {0} has been assigned to another staff.", 
                                request.SystemAccountDto.Identifier ),
                                ErrorLevel.Error );
                        response.SystemAccountDto = request.SystemAccountDto;
                        response.SystemAccountDto.AddDataErrorInfo ( dataErrorInfo );
                    }
                }
                else
                {
                    var dataErrorInfo =
                        new DataErrorInfo ( string.Format ( "Cannot link account because an account with the email {0} does not exist.", request.SystemAccountDto.Identifier ),
                            ErrorLevel.Error );
                    response.SystemAccountDto = request.SystemAccountDto;
                    response.SystemAccountDto.AddDataErrorInfo ( dataErrorInfo );
                }
            }
        }

        #endregion
    }
}