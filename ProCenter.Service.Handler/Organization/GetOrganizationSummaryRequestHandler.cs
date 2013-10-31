namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Get <see cref="Organization" /> summary request handler.
    /// </summary>
    public class GetOrganizationSummaryRequestHandler :
        ServiceRequestHandler<GetDtoByKeyRequest<OrganizationSummaryDto>, DtoResponse<OrganizationSummaryDto>>
    {
        #region Fields

        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetOrganizationSummaryRequestHandler" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        public GetOrganizationSummaryRequestHandler ( IOrganizationRepository organizationRepository )
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
        protected override void Handle ( GetDtoByKeyRequest<OrganizationSummaryDto> request, DtoResponse<OrganizationSummaryDto> response )
        {
            var organization = _organizationRepository.GetByKey ( request.Key );
            response.DataTransferObject = Mapper.Map<Organization, OrganizationSummaryDto> ( organization );
        }

        #endregion
    }
}