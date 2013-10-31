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
    ///     Handler for update organization name requests.
    /// </summary>
    public class UpdateOrganizationNameRequestHandler :
        ServiceRequestHandler<UpdateOrganizationNameRequest, DtoResponse<OrganizationDto>>
    {
        #region Fields

        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateOrganizationNameRequestHandler" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        public UpdateOrganizationNameRequestHandler ( IOrganizationRepository organizationRepository )
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
        protected override void Handle ( UpdateOrganizationNameRequest request, DtoResponse<OrganizationDto> response )
        {
            var organization = _organizationRepository.GetByKey ( request.Key );
            organization.ReviseName ( request.Name );

            response.DataTransferObject = Mapper.Map<Organization, OrganizationDto> ( organization );
        }

        #endregion
    }
}