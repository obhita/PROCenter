namespace ProCenter.Service.Handler.Organization
{
    #region

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    public class CreateStaffRequestHandler : ServiceRequestHandler<CreateStaffRequest, GetStaffDtoResponse>
    {
        protected override void Handle(CreateStaffRequest request, GetStaffDtoResponse response)
        {
            var staff = new StaffFactory().Create(request.OrganizationKey, request.Name);
            if (staff != null)
            {
                var staffDto = Mapper.Map<Staff, StaffDto>(staff);
                response.DataTransferObject = staffDto;
            }
        }
    }
}