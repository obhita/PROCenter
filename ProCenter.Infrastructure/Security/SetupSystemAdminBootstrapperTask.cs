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

namespace ProCenter.Infrastructure.Security
{
    #region Using Statements

    using System;

    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Primitives;
    using Pillar.Security.AccessControl;

    using ProCenter.Common;
    using ProCenter.Domain.SecurityModule;

    #endregion

    /// <summary>The setup system admin bootstrapper task class.</summary>
    public class SetupSystemAdminBootstrapperTask : IOrderedBootstrapperTask
    {
        #region Constants

        private const string SystemAccountIdentifier = "system.admin@feisystems.com";

        #endregion

        #region Fields

        private readonly IRoleFactory _roleFactory;

        private readonly IUnitOfWorkProvider _unitOfWorkProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupSystemAdminBootstrapperTask"/> class.
        /// </summary>
        /// <param name="roleFactory">The role factory.</param>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public SetupSystemAdminBootstrapperTask ( IRoleFactory roleFactory, IUnitOfWorkProvider unitOfWorkProvider )
        {
            _roleFactory = roleFactory;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order
        {
            get { return 1; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute ()
        {
            var systemAccountRepository = IoC.CurrentContainer.Resolve<ISystemAccountRepository> ();
            var systemAccount = systemAccountRepository.GetByIdentifier ( SystemAccountIdentifier );
            if ( systemAccount == null )
            {
                var systemAdminRole = _roleFactory.Create ( "System Admin", null, RoleType.Internal );
                systemAdminRole.AddPermision ( SystemAdministrationPermission.SystemAdminPermission );
                systemAdminRole.AddPermision ( new Permission { Name = "infrastructuremodule/accessuserinterface" } );

                systemAccount = new SystemAccount ( Guid.Empty, SystemAccountIdentifier, new Email ( SystemAccountIdentifier ) );
                systemAccount.AddRole ( systemAdminRole.Key );

                _unitOfWorkProvider.GetCurrentUnitOfWork ().Commit ();
            }
        }

        #endregion
    }
}