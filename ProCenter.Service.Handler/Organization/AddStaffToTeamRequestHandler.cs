namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    /// <summary>
    ///     Handler for adding staff to team.
    /// </summary>
    public class AddStaffToTeamRequestHandler : ServiceRequestHandler<AddDtoRequest<TeamStaffDto>, DtoResponse<TeamStaffDto>>
    {
        #region Fields

        private readonly ITeamRepository _teamRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddStaffToTeamRequestHandler" /> class.
        /// </summary>
        /// <param name="teamRepository">The team repository.</param>
        public AddStaffToTeamRequestHandler ( ITeamRepository teamRepository )
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
        protected override void Handle ( AddDtoRequest<TeamStaffDto> request, DtoResponse<TeamStaffDto> response )
        {
            var team = _teamRepository.GetByKey ( request.AggregateKey );
            team.AddStaff ( request.DataTransferObject.Key );

            response.DataTransferObject = request.DataTransferObject;
        }

        #endregion
    }
}