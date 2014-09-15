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
    using Common;
    using Domain.SecurityModule;
    using global::AutoMapper;
    using Infrastructure.Service;
    using NLog;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Security;

    #endregion

    /// <summary>Handler for creating a system admin.</summary>
    public class CreateSystemAdminRequestHandler : ServiceRequestHandler<CreateSystemAdminRequest, DtoResponse<SystemAccountDto>>
    {
        #region Fields
        
        private readonly IRoleRepository _roleRepository;
        private readonly ISystemAccountIdentityServiceManager _systemAccountIdentityServiceManager;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateSystemAdminRequestHandler" /> class.
        /// </summary>
        /// <param name="systemAccountRepository">The system account repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="systemAccountIdentityServiceManager">The system account identity service manager.</param>
        public CreateSystemAdminRequestHandler ( ISystemAccountRepository systemAccountRepository,
            IRoleRepository roleRepository,
            ISystemAccountIdentityServiceManager systemAccountIdentityServiceManager )
        {
            _systemAccountRepository = systemAccountRepository;
            _roleRepository = roleRepository;
            _systemAccountIdentityServiceManager = systemAccountIdentityServiceManager;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( CreateSystemAdminRequest request, DtoResponse<SystemAccountDto> response )
        {
            var systemAccount = _systemAccountRepository.GetByIdentifier ( request.Email );
            var roleKey = _roleRepository.GetInternalRoleKeyByName ( "System Admin" );
            if ( roleKey.HasValue )
            {
                if ( systemAccount == null )
                {
                    var result = _systemAccountIdentityServiceManager.Create ( request.Email );
                    if ( result.Sucess )
                    {
                        var systemAccountFactory = new SystemAccountFactory ();
                        systemAccount = systemAccountFactory.Create ( Guid.Empty, request.Email, new Email ( request.Email ) );
                        systemAccount.AddRole ( roleKey.Value );
                        var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto> ( systemAccount );
                        response.DataTransferObject = systemAccountDto;
                    }
                    else
                    {
                        var dataErrorInfo = new DataErrorInfo ( result.ErrorMessage, ErrorLevel.Error );
                        response.DataTransferObject = new SystemAccountDto ();
                        response.DataTransferObject.AddDataErrorInfo ( dataErrorInfo );
                    }
                }
                else
                {
                    var dataErrorInfo = new DataErrorInfo ( "System Admin already exists.", ErrorLevel.Error );
                    response.DataTransferObject = new SystemAccountDto ();
                    response.DataTransferObject.AddDataErrorInfo ( dataErrorInfo );
                }
            }
        }

        #endregion
    }
}