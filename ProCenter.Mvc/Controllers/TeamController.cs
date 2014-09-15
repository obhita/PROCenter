#region License Header

// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

#endregion

namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Primitive;
    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    /// <summary>The team controller class.</summary>
    public class TeamController : BaseController
    {
        #region Fields

        private readonly IDbConnectionFactory _dbConnectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public TeamController ( IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory dbConnectionFactory )
            : base ( requestDispatcherFactory )
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the patients.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKeysToAdd">The patient keys to add.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> AddPatients ( Guid key, Guid[] patientKeysToAdd )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var patientKey in patientKeysToAdd )
            {
                requestDispatcher.Add (
                                       patientKey.ToString (),
                    new AddDtoRequest<TeamPatientDto> { AggregateKey = key, DataTransferObject = new TeamPatientDto { Key = patientKey } } );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = patientKeysToAdd};
        }

        /// <summary>
        /// Adds the staff.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="staffKeysToAdd">The staff keys to add.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> AddStaff ( Guid key, Guid[] staffKeysToAdd )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var staffKey in staffKeysToAdd )
            {
                requestDispatcher.Add ( staffKey.ToString (), new AddDtoRequest<TeamStaffDto> {AggregateKey = key, DataTransferObject = new TeamStaffDto {Key = staffKey}} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = staffKeysToAdd};
        }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public async Task<PartialViewResult> Create ( string name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new CreateTeamRequest {Name = name, OrganizationKey = UserContext.Current.OrganizationKey.Value} );
            var response = await requestDispatcher.GetAsync<DtoResponse<TeamSummaryDto>> ();
            return PartialView ( "Edit", new TeamDto {Key = response.DataTransferObject.Key, Name = response.DataTransferObject.Name} );
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="PartialViewResult"/>.</returns>
        public PartialViewResult Edit ( Guid key )
        {
            const string Query = @"
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

            using ( var connection = _dbConnectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( Query, new {Key = key} ) )
            {
                var teamDto = multiQuery.Read<TeamDto> ().Single ();

                teamDto.Staff = multiQuery.Read<TeamStaffDto, PersonName, TeamStaffDto> ( ( teamStaffDto, personName ) =>
                {
                    teamStaffDto.Name = personName;
                    return teamStaffDto;
                },
                    "FirstName" ).ToList ();

                teamDto.Patients = multiQuery.Read<TeamPatientDto, PersonName, TeamPatientDto> ( ( teamPatientDto, personName ) =>
                {
                    teamPatientDto.Name = personName;
                    return teamPatientDto;
                },
                    "FirstName" ).ToList ();

                return PartialView ( teamDto );
            }
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit ( Guid key, string name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new UpdateTeamNameRequest {Key = key, Name = name} );
            var response = await requestDispatcher.GetAsync<DtoResponse<TeamSummaryDto>> ();

            return new JsonResult {Data = new {}};
        }

        /// <summary>
        /// Removes the patients.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKeysToRemove">The patient keys to remove.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> RemovePatients ( Guid key, Guid[] patientKeysToRemove )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var patientKey in patientKeysToRemove )
            {
                requestDispatcher.Add ( patientKey.ToString (), new RemovePatientFromTeamRequest {TeamKey = key, PatientKey = patientKey} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = patientKeysToRemove};
        }

        /// <summary>
        /// Removes the staff.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="staffKeysToRemove">The staff keys to remove.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> RemoveStaff ( Guid key, Guid[] staffKeysToRemove )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            foreach ( var staffKey in staffKeysToRemove )
            {
                requestDispatcher.Add ( staffKey.ToString (), new RemoveStaffFromTeamRequest {TeamKey = key, StaffKey = staffKey} );
            }
            await requestDispatcher.GetAllAsync ();

            return new JsonResult {Data = staffKeysToRemove};
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iRow">The i row.</param>
        /// <returns>
        /// Returns an ActionResult.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Remove(Guid key, int iRow)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new RemoveTeamRequest { TeamKey = key });
            var response = await requestDispatcher.GetAsync<DtoResponse<TeamSummaryDto>>();
            if (response.DataTransferObject.DataErrorInfoCollection.Any())
            {
                return new JsonResult
                {
                    Data = new
                    {
                        error = true,
                        errors = response.DataTransferObject.DataErrorInfoCollection
                    }
                };
            }
            return new JsonResult
            {
                Data = new { iRow },
            };
        }

        #endregion
    }
}