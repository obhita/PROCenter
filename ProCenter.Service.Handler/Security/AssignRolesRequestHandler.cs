namespace ProCenter.Service.Handler.Security
{
    using Common;
    using Domain.SecurityModule;
    using Service.Message.Security;

    public class AssignRolesRequestHandler : ServiceRequestHandler<AssignRolesRequest, AssignRolesResponse>
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        public AssignRolesRequestHandler(ISystemAccountRepository systemAccountRepository )
        {
            _systemAccountRepository = systemAccountRepository;
        }

        protected override void Handle(AssignRolesRequest request, AssignRolesResponse response)
        {
            var systemAccount = _systemAccountRepository.GetByKey(request.SystemAccoutnKey);

            if (systemAccount != null)
            {
                if (request.AddRoles)
                {
                    foreach (var role in request.Roles)
                    {
                        systemAccount.AddRole(role);
                    }
                }
                else
                {
                    foreach (var role in request.Roles)
                    {
                        systemAccount.RemoveRole(role);
                    }
                }
            }
        }
    }
}