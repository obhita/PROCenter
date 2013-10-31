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
    using System.Data;
    using Dapper;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;
    using Primitive;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.OrganizationModule.Event;

    public class StaffUpdater : IHandleMessages<StaffCreatedEvent>,
                                IHandleMessages<StaffChangedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StaffUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(StaffChangedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, PersonName>(s=>s.Name))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    var name = (message.Value as PersonName);
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET FirstName = @FirstName, LastName = @LastName WHERE StaffKey = @StaffKey",
                        new {name.FirstName, name.LastName, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, Email>(s => s.Email))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    var email = (Email) message.Value;
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET Email = @Email WHERE StaffKey = @StaffKey",
                        new {Email = email == null ? (string) null : email.Address, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, string>(s => s.Location))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET Location = @Location WHERE StaffKey = @StaffKey",
                        new {Location = (string)message.Value, StaffKey = message.Key});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<Staff, string>(s => s.NPI))
            {
                using (IDbConnection connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "UPDATE OrganizationModule.Staff SET NPI = @NPI WHERE StaffKey = @StaffKey",
                        new {NPI= (string)message.Value, StaffKey = message.Key});
                }
            }
        }

        public void Handle(StaffCreatedEvent message)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "INSERT INTO OrganizationModule.Staff(StaffKey, OrganizationKey, FirstName, LastName) VALUES(@StaffKey, @OrganizationKey, @FirstName, @LastName)",
                    new
                        {
                            StaffKey = message.Key,
                            message.OrganizationKey,
                            message.Name.FirstName,
                            message.Name.LastName,
                        });
            }
        }
    }
}