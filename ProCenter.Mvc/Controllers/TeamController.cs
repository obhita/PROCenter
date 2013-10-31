namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Models;
    using Primitive;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    public class TeamController : BaseController
    {
        #region Fields

        private readonly IDbConnectionFactory _dbConnectionFactory;

        #endregion

        #region Constructors and Destructors

        public TeamController ( IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory dbConnectionFactory )
            : base ( requestDispatcherFactory )
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        public async Task<ActionResult> AddPatients ( Guid key, Guid[] patientKeysToAdd )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var patientKey in patientKeysToAdd )
            {
                requestDispatcher.Add ( patientKey.ToString(), new AddDtoRequest<TeamPatientDto> {AggregateKey = key, DataTransferObject = new TeamPatientDto {Key = patientKey}} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = patientKeysToAdd};
        }

        [HttpPost]
        public async Task<ActionResult> AddStaff ( Guid key, Guid[] staffKeysToAdd )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var staffKey in staffKeysToAdd )
            {
                requestDispatcher.Add ( staffKey.ToString(), new AddDtoRequest<TeamStaffDto> {AggregateKey = key, DataTransferObject = new TeamStaffDto {Key = staffKey}} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = staffKeysToAdd};
        }

        public async Task<PartialViewResult> Create ( string name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new CreateTeamRequest {Name = name, OrganizationKey = UserContext.Current.OrganizationKey.Value} );
            var response = await requestDispatcher.GetAsync<DtoResponse<TeamSummaryDto>> ();
            return PartialView ( "Edit", new TeamDto {Key = response.DataTransferObject.Key, Name = response.DataTransferObject.Name} );
        }

        public PartialViewResult Edit(Guid key)
        {
            const string query = @"
                            SELECT 
                                 TeamKey AS 'Key',
                                 Name 
                             FROM OrganizationModule.Team
                             WHERE TeamKey=@Key 
                             SELECT                                  
                                 StaffKey AS 'Key',
                                 FirstName, 
                                 LastName
                             FROM OrganizationModule.TeamStaff 
                             WHERE TeamKey=@Key 
                             SELECT                           
                                 PatientKey as 'Key', 
                                 FirstName,  
                                 LastName 
                             FROM OrganizationModule.TeamPatient 
                             WHERE TeamKey=@Key";

            using (var connection = _dbConnectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(query, new {Key = key}))
            {
                var teamDto = multiQuery.Read<TeamDto>().Single();

                teamDto.Staff = multiQuery.Read<TeamStaffDto, PersonName, TeamStaffDto>((teamStaffDto, personName) =>
                    {
                        teamStaffDto.Name = personName;
                        return teamStaffDto;
                    }, "FirstName").ToList();

                teamDto.Patients = multiQuery.Read<TeamPatientDto, PersonName, TeamPatientDto>((teamPatientDto, personName) =>
                    {
                        teamPatientDto.Name = personName;
                        return teamPatientDto;
                    }, "FirstName").ToList();

                return PartialView(teamDto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit ( Guid key, string name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new UpdateTeamNameRequest {Key = key, Name = name} );
            var response = await requestDispatcher.GetAsync<DtoResponse<TeamSummaryDto>> ();

            return new JsonResult {Data = new {}};
        }

        [HttpPost]
        public async Task<ActionResult> RemovePatients ( Guid key, Guid[] patientKeysToRemove )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var patientKey in patientKeysToRemove )
            {
                requestDispatcher.Add ( patientKey.ToString(), new RemovePatientFromTeamRequest {TeamKey = key, PatientKey = patientKey} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = patientKeysToRemove};
        }

        [HttpPost]
        public async Task<ActionResult> RemoveStaff ( Guid key, Guid[] staffKeysToRemove )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var staffKey in staffKeysToRemove )
            {
                requestDispatcher.Add ( staffKey.ToString(), new RemoveStaffFromTeamRequest {TeamKey = key, StaffKey = staffKey} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = staffKeysToRemove};
        }

        #endregion
    }
}