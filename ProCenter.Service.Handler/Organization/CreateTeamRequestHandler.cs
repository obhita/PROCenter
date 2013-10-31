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
    ///     Handler for creating a <see cref="Team" />.
    /// </summary>
    public class CreateTeamRequestHandler : ServiceRequestHandler<CreateTeamRequest, DtoResponse<TeamSummaryDto>>
    {
        #region Fields

        private readonly ITeamFactory _teamFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTeamRequestHandler" /> class.
        /// </summary>
        /// <param name="teamFactory">The team factory.</param>
        public CreateTeamRequestHandler ( ITeamFactory teamFactory )
        {
            _teamFactory = teamFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( CreateTeamRequest request, DtoResponse<TeamSummaryDto> response )
        {
            var team = _teamFactory.Create ( request.OrganizationKey, request.Name );
            if ( team != null )
            {
                response.DataTransferObject = Mapper.Map<Team, TeamSummaryDto> ( team );
            }
        }

        #endregion
    }
}