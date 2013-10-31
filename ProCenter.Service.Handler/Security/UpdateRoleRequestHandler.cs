namespace ProCenter.Service.Handler.Security
{
    #region

    using Common;
    using Domain.SecurityModule;
    using Service.Message.Common;
    using Service.Message.Security;
    using global::AutoMapper;

    #endregion

    public class UpdateRoleRequestHandler : ServiceRequestHandler<UpdateRoleRequest, DtoResponse<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        protected override void Handle(UpdateRoleRequest request, DtoResponse<RoleDto> response)
        {
            var role = _roleRepository.GetByKey(request.Key);
            if (role != null)
            {
                role.ReviseName(request.Name);

                var roleDto = Mapper.Map<Role, RoleDto>(role);
                response.DataTransferObject = roleDto;
            }
        }
    }
}