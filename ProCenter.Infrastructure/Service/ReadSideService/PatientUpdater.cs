#region Licence Header
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
namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using Dapper;
    using Pillar.Common.Utility;
    using Primitive;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;

    #endregion

    /// <summary>
    ///     Handles updating patient table.
    /// </summary>
    public class PatientUpdater : IHandleMessages<PatientCreatedEvent>,
                                  IHandleMessages<PatientChangedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PatientUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(PatientChangedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<Patient, Gender>(p => p.Gender))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE PatientModule.Patient SET GenderCode = @GenderCode WHERE PatientKey=@PatientKey",
                        new {GenderCode = (message.Value as Lookup).CodedConcept.Code, PatientKey = message.Key});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName>(p => p.Name))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var name = (message.Value as PersonName);
                    connection.Execute(
                        "UPDATE PatientModule.Patient SET FirstName = @FirstName, LastName = @LastName WHERE PatientKey=@PatientKey",
                        new {name.FirstName, name.LastName, PatientKey = message.Key});
                }
            }
        }

        /// <summary>
        ///     Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(PatientCreatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "INSERT INTO PatientModule.Patient VALUES(@PatientKey, @OrganizationKey, @GenderCode, @FirstName, @LastName, @UniqueIdentifier)",
                    new
                        {
                            PatientKey = message.Key,
                            message.OrganizationKey,
                            message.Name.FirstName,
                            message.Name.LastName,
                            GenderCode = message.Gender.CodedConcept.Code,
                            message.UniqueIdentifier,
                        });
            }
        }

        #endregion
    }
}