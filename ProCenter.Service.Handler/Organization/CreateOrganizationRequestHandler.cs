namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using Common;
    using global::AutoMapper;
    using Service.Message.Common;
    using Service.Message.Organization;
    using ProCenter.Domain.OrganizationModule;

    #endregion

    /// <summary>The create organization request handler class.</summary>
    public class CreateOrganizationRequestHandler :
        ServiceRequestHandler<CreateOrganizationRequest, DtoResponse<OrganizationSummaryDto>>
    {
        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( CreateOrganizationRequest request, DtoResponse<OrganizationSummaryDto> response )
        {
            var organization = new Organization ( request.Name );
            response.DataTransferObject = Mapper.Map<Organization, OrganizationSummaryDto> ( organization );
        }

        #endregion
    }
}