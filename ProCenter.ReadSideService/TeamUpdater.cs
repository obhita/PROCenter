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

namespace ProCenter.ReadSideService
{
    #region Using Statements

    using Dapper;

    using Pillar.Common.Utility;

    using ProCenter.Common;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.OrganizationModule.Event;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;
    using ProCenter.Primitive;

    #endregion

    /// <summary>The team updater class.</summary>
    public class TeamUpdater :
        IHandleMessages<TeamCreatedEvent>,
        IHandleMessages<StaffAddedToTeamEvent>,
        IHandleMessages<PatientAddedToTeamEvent>,
        IHandleMessages<StaffRemovedFromTeamEvent>,
        IHandleMessages<PatientRemovedFromTeamEvent>,
        IHandleMessages<PatientChangedEvent>,
        IHandleMessages<StaffChangedEvent>,
        IHandleMessages<TeamNameRevisedEvent>,
        IHandleMessages<TeamRemovedEvent>
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IPatientRepository _patientRepository;

        private readonly IStaffRepository _staffRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamUpdater"/> class.
        /// </summary>
        /// <param name="staffRepository">The staff repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        public TeamUpdater ( IStaffRepository staffRepository, IPatientRepository patientRepository, IDbConnectionFactory connectionFactory )
        {
            _staffRepository = staffRepository;
            _patientRepository = patientRepository;
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( TeamCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "insert into OrganizationModule.Team(TeamKey, OrganizationKey, Name) values(@TeamKey, @OrganizationKey, @Name)",
                    new
                    {
                        TeamKey = message.Key,
                        message.OrganizationKey,
                        message.Name,
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( StaffAddedToTeamEvent message )
        {
            var staff = _staffRepository.GetByKey ( message.StaffKey );
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "insert into OrganizationModule.TeamStaff(TeamStaffKey, TeamKey, StaffKey, FirstName, LastName, OrganizationKey) " +
                                    "values(@TeamStaffKey,@TeamKey, @StaffKey, @FirstName, @LastName, @OrganizationKey)",
                    new
                    {
                        TeamStaffKey = CombGuid.NewCombGuid (),
                        TeamKey = message.Key,
                        message.StaffKey,
                        staff.Name.FirstName,
                        staff.Name.LastName,
                        message.OrganizationKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( PatientAddedToTeamEvent message )
        {
            var patient = _patientRepository.GetByKey ( message.PatientKey );
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "insert into OrganizationModule.TeamPatient(TeamPatientKey, TeamKey, PatientKey, FirstName, LastName, OrganizationKey) " +
                                    "values(@TeamPatientKey,@TeamKey, @PatientKey, @FirstName, @LastName, @OrganizationKey)",
                    new
                    {
                        TeamPatientKey = CombGuid.NewCombGuid (),
                        TeamKey = message.Key,
                        message.PatientKey,
                        patient.Name.FirstName,
                        patient.Name.LastName,
                        message.OrganizationKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( PatientChangedEvent message )
        {
            if ( message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName> ( patient => patient.Name ) )
            {
                var name = message.Value as PersonName;
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        "UPDATE OrganizationModule.TeamPatient SET FirstName = @FirstName, LastName = @LastName WHERE PatientKey = @PatientKey",
                        new
                        {
                            PatientKey = message.Key,
                            name.FirstName,
                            name.LastName
                        } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( StaffChangedEvent message )
        {
            if ( message.Property == PropertyUtil.ExtractPropertyName<Staff, PersonName> ( s => s.Name ) )
            {
                var name = message.Value as PersonName;
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        "UPDATE OrganizationModule.TeamStaff SET FirstName = @FirstName, LastName = @LastName WHERE StaffKey = @StaffKey",
                        new
                        {
                            StaffKey = message.Key,
                            name.FirstName,
                            name.LastName
                        } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( StaffRemovedFromTeamEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "DELETE FROM OrganizationModule.TeamStaff WHERE StaffKey = @StaffKey",
                    new
                    {
                        message.StaffKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( PatientRemovedFromTeamEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "DELETE FROM OrganizationModule.TeamPatient WHERE PatientKey = @PatientKey",
                    new
                    {
                        message.PatientKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( TeamNameRevisedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "UPDATE OrganizationModule.Team Set Name = @Name WHERE TeamKey = @TeamKey",
                    new
                    {
                        TeamKey = message.Key,
                        Name = message.Name,
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(TeamRemovedEvent message)
        {
            const string SqlTeamDelete = "DELETE FROM OrganizationModule.Team " +
                               "WHERE OrganizationModule.Team.TeamKey = @TeamKey";
            const string SqlTeamPatientDelete = "DELETE FROM OrganizationModule.TeamPatient " +
                               "WHERE OrganizationModule.TeamPatient.TeamKey = @TeamKey";
            const string SqlTeamStaffDelete = "DELETE FROM OrganizationModule.TeamStaff " +
                               "WHERE OrganizationModule.TeamStaff.TeamKey = @TeamKey";

            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute( SqlTeamDelete, new { TeamKey = message.Key, });
                connection.Execute( SqlTeamPatientDelete, new { TeamKey = message.Key, });
                connection.Execute( SqlTeamStaffDelete, new { TeamKey = message.Key, });
            }
        }

        #endregion
    }
}