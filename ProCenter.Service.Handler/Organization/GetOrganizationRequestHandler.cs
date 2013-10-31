namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    public class GetOrganizationRequestHandler :
        ServiceRequestHandler<GetDtoByKeyRequest<OrganizationDto>, DtoResponse<OrganizationDto>>
    {
        #region Fields

        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetOrganizationSummaryRequestHandler" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        public GetOrganizationRequestHandler ( IOrganizationRepository organizationRepository )
        {
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetDtoByKeyRequest<OrganizationDto> request, DtoResponse<OrganizationDto> response )
        {
            var organization = _organizationRepository.GetByKey ( request.Key );
            response.DataTransferObject = Mapper.Map<Organization, OrganizationDto> ( organization );
        }

        #endregion
    }
}