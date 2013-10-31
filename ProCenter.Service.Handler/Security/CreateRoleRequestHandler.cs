namespace ProCenter.Service.Handler.Security
{
    #region

    using Common;
    using Domain.SecurityModule;
    using Service.Message.Security;
    using global::AutoMapper;

    #endregion

    public class CreateRoleRequestHandler : ServiceRequestHandler<CreateRoleRequest, CreateRoleResponse>
    {
        protected override void Handle(CreateRoleRequest request, CreateRoleResponse response)
        {
            var roleFactory = new RoleFactory();
            var role = roleFactory.Create(request.Name);
            var roleDto = Mapper.Map<Role, RoleDto>(role);
            response.Role = roleDto;
        }
    }
}