namespace ProCenter.Service.Handler.Organization
{
    #region

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    #endregion

    public class UpdateTeamNameRequestHandler :
        ServiceRequestHandler<UpdateTeamNameRequest, DtoResponse<TeamSummaryDto>>
    {
        #region Fields

        private readonly ITeamRepository _teamRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateTeamNameRequestHandler" /> class.
        /// </summary>
        /// <param name="teamRepository">The team repository.</param>
        public UpdateTeamNameRequestHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle(UpdateTeamNameRequest request, DtoResponse<TeamSummaryDto> response)
        {
            var team = _teamRepository.GetByKey(request.Key);
            team.ReviseName(request.Name);

            response.DataTransferObject = Mapper.Map<Team, TeamSummaryDto>(team);
        }

        #endregion
    }
}