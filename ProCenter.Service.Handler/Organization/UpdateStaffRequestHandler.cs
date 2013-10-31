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
namespace ProCenter.Service.Handler.Organization
{
    using Common;
    using Domain.OrganizationModule;
    using Pillar.Domain.Primitives;
    using Primitive;
    using Service.Message.Common;
    using Service.Message.Organization;
    using global::AutoMapper;

    public class UpdateStaffRequestHandler: ServiceRequestHandler<UpdateStaffRequest, DtoResponse<StaffDto> >
    {
        private readonly IStaffRepository _staffRepository;

        public UpdateStaffRequestHandler(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        protected override void Handle(UpdateStaffRequest request, DtoResponse<StaffDto> response)
        {
            var staff = _staffRepository.GetByKey(request.StaffKey);
            switch (request.UpdateType)
            {
                case UpdateStaffRequest.StaffUpdateType.Name:
                    staff.ReviseName((PersonName)request.Value);
                    break;
                case UpdateStaffRequest.StaffUpdateType.Email:
                    staff.ReviseEmail(string.IsNullOrWhiteSpace((string)request.Value) ? null : new Email((string)request.Value));
                    break;
                case UpdateStaffRequest.StaffUpdateType.Location:
                    staff.ReviseLocation((string)request.Value);
                    break;
                case UpdateStaffRequest.StaffUpdateType.NPI:
                    staff.ReviseNpi((string)request.Value);
                    break;
            }
            response.DataTransferObject = Mapper.Map<Staff, StaffDto>(staff);
        }
    }
}