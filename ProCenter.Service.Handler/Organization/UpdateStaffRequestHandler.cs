namespace ProCenter.Service.Handler.Organization
{
    using Common;
    using Domain.OrganizationModule;
    using Pillar.Domain.Primitives;
    using Primitive;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    public class UpdateStaffRequestHandler: ServiceRequestHandler<UpdateStaffRequest, DtoResponse<StaffDto> >
    {
        private readonly IStaffRepository _staffRepository;

        public UpdateStaffRequestHandler(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        protected override void Handle(UpdateStaffRequest request, DtoResponse<StaffDto> response)
        {
            var staff = _staffRepository.GetByKey(request.StaffKey);
            switch (request.UpdateType)
            {
                case UpdateStaffRequest.StaffUpdateType.Name:
                    staff.ReviseName((PersonName)request.Value);
                    break;
                case UpdateStaffRequest.StaffUpdateType.Email:
                    staff.ReviseEmail(string.IsNullOrWhiteSpace((string)request.Value) ? null : new Email((string)request.Value));
                    break;
                case UpdateStaffRequest.StaffUpdateType.Location:
                    staff.ReviseLocation((string)request.Value);
                    break;
                case UpdateStaffRequest.StaffUpdateType.NPI:
                    staff.ReviseNpi((string)request.Value);
                    break;
            }
            response.DataTransferObject = Mapper.Map<Staff, StaffDto>(staff);
        }
    }
}