namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using System.Linq;
    using Common;
    using Domain.CommonModule;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Handler for request to add <see cref="Phone" /> to <see cref="Organization" />.
    /// </summary>
    public class AddPhoneToOrganizationRequestHandler :
        ServiceRequestHandler<AddDtoRequest<OrganizationPhoneDto>, AddDtoResponse<OrganizationPhoneDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;
        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddPhoneToOrganizationRequestHandler" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        public AddPhoneToOrganizationRequestHandler ( IOrganizationRepository organizationRepository, ILookupProvider lookupProvider )
        {
            _organizationRepository = organizationRepository;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle(AddDtoRequest<OrganizationPhoneDto> request, AddDtoResponse<OrganizationPhoneDto> response)
        {
            var organization = _organizationRepository.GetByKey(request.AggregateKey);
            var originalPhone = organization.OrganizationPhones.FirstOrDefault(p => p.GetHashCode() == request.DataTransferObject.OriginalHash); var organizationPhoneType = _lookupProvider.Find<OrganizationPhoneType>(request.DataTransferObject.OrganizationPhoneType.Code);
            var phone = new Phone(request.DataTransferObject.Phone.Number, request.DataTransferObject.Phone.Extension);

            var organizationPhone = new OrganizationPhone(organizationPhoneType, phone, request.DataTransferObject.IsPrimary);
            if (originalPhone != organizationPhone)
            {
                if ( originalPhone != null )
                {
                    organization.RemovePhone ( originalPhone );
                }
                organization.AddPhone ( organizationPhone );
            }
            else if ( organizationPhone.IsPrimary )
            {
                organization.MakePrimary ( organizationPhone );
            }

            response.AggregateKey = organization.Key;
            response.DataTransferObject = Mapper.Map<OrganizationPhone, OrganizationPhoneDto>(organizationPhone);
            response.DataTransferObject.Key = organization.Key;
        }

        #endregion
    }
}