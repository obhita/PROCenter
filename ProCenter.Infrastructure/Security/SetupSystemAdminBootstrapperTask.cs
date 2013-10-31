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
