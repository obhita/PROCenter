namespace ProCenter.Service.Handler.Organization
{
    #region Using Statements

    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    /// <summary>
    ///     Handler for adding patient to team.
    /// </summary>
    public class AddPatientToTeamRequestHandler : ServiceRequestHandler<AddDtoRequest<TeamPatientDto>, DtoResponse<TeamPatientDto>>
    {
        #region Fields

        private readonly ITeamRepository _teamRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddPatientToTeamRequestHandler" /> class.
        /// </summary>
        /// <param name="teamRepository">The team repository.</param>
        public AddPatientToTeamRequestHandler ( ITeamRepository teamRepository )
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
        protected override void Handle ( AddDtoRequest<TeamPatientDto> request, DtoResponse<TeamPatientDto> response )
        {
            var team = _teamRepository.GetByKey ( request.AggregateKey );
            team.AddPatient ( request.DataTransferObject.Key );

            response.DataTransferObject = request.DataTransferObject;
        }

        #endregion
    }
}