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

    using ProCenter.Common;
    using ProCenter.Domain.SecurityModule.Event;

    #endregion

    /// <summary>The system acount updater class.</summary>
    public class SystemAcountUpdater : IHandleMessages<AssignedStaffToSystemAccountEvent>,
        IHandleMessages<SystemAccountCreatedEvent>,
        IHandleMessages<SystemAccountRoleRemovedEvent>,
        IHandleMessages<SystemAccountRoleAddedEvent>,
        IHandleMessages<AssignedPatientToSystemAccountEvent>
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAcountUpdater"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public SystemAcountUpdater ( IDbConnectionFactory connectionFactory )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssignedStaffToSystemAccountEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "update SecurityModule.SystemAccount set StaffKey = @StaffKey where SystemAccountKey = @SystemAccountKey",
                    new
                    {
                        message.StaffKey,
                        SystemAccountKey = message.Key,
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssignedPatientToSystemAccountEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "update SecurityModule.SystemAccount set PatientKey = @PatientKey where SystemAccountKey = @SystemAccountKey",
                    new
                    {
                        message.PatientKey,
                        SystemAccountKey = message.Key,
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( SystemAccountCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "insert into SecurityModule.SystemAccount (SystemAccountKey, OrganizationKey, [Identifier], Email) " +
                                    "values (@SystemAccountKey, @OrganizationKey, @Identifier, @Email)",
                    new
                    {
                        SystemAccountKey = message.Key,
                        message.OrganizationKey,
                        message.Identifier,
                        Email = message.Email.Address,
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( SystemAccountRoleRemovedEvent message )
        {
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( SystemAccountRoleAddedEvent message )
        {
        }

        #endregion
    }
}