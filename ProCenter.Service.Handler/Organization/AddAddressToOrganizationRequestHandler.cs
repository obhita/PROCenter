namespace ProCenter.Service.Handler.Organization
{
    #region

    using System.Linq;
    using Common;
    using Domain.CommonModule;
    using Domain.OrganizationModule;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    public class AddAddressToOrganizationRequestHandler :
        ServiceRequestHandler<AddDtoRequest<OrganizationAddressDto>, AddDtoResponse<OrganizationAddressDto>>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ILookupProvider _lookupProvider;

        public AddAddressToOrganizationRequestHandler(IOrganizationRepository organizationRepository, ILookupProvider lookupProvider)
        {
            _organizationRepository = organizationRepository;
            _lookupProvider = lookupProvider;
        }

        protected override void Handle(AddDtoRequest<OrganizationAddressDto> request, AddDtoResponse<OrganizationAddressDto> response)
        {
            var organization = _organizationRepository.GetByKey(request.AggregateKey);
            var addressDto = request.DataTransferObject.Address;
            var address = new Address(addressDto.FirstStreetAddress,
                                      addressDto.SecondStreetAddress,
                                      addressDto.CityName,
                                      _lookupProvider.Find<StateProvince>(addressDto.StateProvince.Code),
                                      new PostalCode(addressDto.PostalCode));

            var organizationAddressType = _lookupProvider.Find<OrganizationAddressType>(request.DataTransferObject.OrganizationAddressType.Code);
            var organizationAddress = new OrganizationAddress(organizationAddressType, address, request.DataTransferObject.IsPrimary);
            var originalAddress = organization.OrganizationAddresses.FirstOrDefault(a => a.GetHashCode() == request.DataTransferObject.OriginalHash);
            if (originalAddress != organizationAddress)
            {
                if (originalAddress != null)
                {
                    organization.RemoveAddress(originalAddress);
                }
                organization.AddAddress(organizationAddress);
            }
            else if (organizationAddress.IsPrimary)
            {
                organization.MakePrimary(organizationAddress);
            }
            response.AggregateKey = organization.Key;
            response.DataTransferObject = Mapper.Map<OrganizationAddress, OrganizationAddressDto>(organizationAddress);
            response.DataTransferObject.Key = organization.Key;
        }
    }
}