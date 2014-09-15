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
    using ProCenter.Domain.OrganizationModule.Event;

    #endregion

    /// <summary>The organization updater class.</summary>
    public class OrganizationUpdater : IHandleMessages<AssessmentDefinitionAddedEvent>,
        IHandleMessages<AssessmentDefinitionRemovedEvent>,
        IHandleMessages<OrganizationCreatedEvent>,
        IHandleMessages<OrganizationNameRevisedEvent>
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUpdater"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        public OrganizationUpdater ( IDbConnectionFactory connectionFactory )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentDefinitionAddedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                    @"INSERT INTO [OrganizationModule].[OrganizationAssessmentDefinition] ([OrganizationKey], [AssessmentDefinitionKey], [AssessmentName], [AssessmentCode], [ScoreType]) 
                    SELECT @OrganizationKey, @AssessmentDefinitionKey, a.AssessmentName, a.AssessmentCode, a.ScoreType
                    FROM AssessmentModule.AssessmentDefinition a 
                    WHERE AssessmentDefinitionKey = @AssessmentDefinitionKey",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.AssessmentDefinitionKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentDefinitionRemovedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "DELETE FROM OrganizationModule.OrganizationAssessmentDefinition " +
                                    "WHERE OrganizationKey = @OrganizationKey AND AssessmentDefinitionKey = @AssessmentDefinitionKey",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.AssessmentDefinitionKey
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( OrganizationCreatedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "INSERT INTO OrganizationModule.Organization(OrganizationKey, Name) VALUES(@OrganizationKey, @Name)",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.Name
                    } );
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( OrganizationNameRevisedEvent message )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                connection.Execute (
                                    "UPDATE OrganizationModule.Organization Set Name = @Name WHERE OrganizationKey = @OrganizationKey",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.Name,
                    } );
            }
        }

        #endregion
    }
}