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
namespace ProCenter.Infrastructure.Security
{
    using System;
    using Common;
    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Primitives;
    using Pillar.Security.AccessControl;
    using ProCenter.Domain.SecurityModule;

    public class SetupSystemAdminBootstrapperTask : IOrderedBootstrapperTask
    {
        private readonly IRoleFactory _roleFactory;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;

        private const string systemAccountIdentifier = "system.admin@feisystems.com";

        public int Order { get { return 1; } }

        public SetupSystemAdminBootstrapperTask (IRoleFactory roleFactory, IUnitOfWorkProvider unitOfWorkProvider )
        {
            _roleFactory = roleFactory;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        public void Execute ()
        {
            var systemAccountRepository = IoC.CurrentContainer.Resolve<ISystemAccountRepository> ();
            var systemAccount = systemAccountRepository.GetByIdentifier(systemAccountIdentifier);
            if ( systemAccount == null )
            {
                var systemAdminRole = _roleFactory.Create ( "System Admin", RoleType.Internal );
                systemAdminRole.AddPermision(SystemAdministrationPermission.SystemAdminPermission);
                systemAdminRole.AddPermision(new Permission { Name = "infrastructuremodule/accessuserinterface" });

                systemAccount = new SystemAccount(Guid.Empty, systemAccountIdentifier, new Email(systemAccountIdentifier));
                systemAccount.AddRole ( systemAdminRole.Key );

                _unitOfWorkProvider.GetCurrentUnitOfWork ().Commit ();
            }
        }
    }
}
