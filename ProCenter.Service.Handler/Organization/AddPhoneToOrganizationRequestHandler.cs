﻿#region License Header

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
    #region Using Statements

    using System.Linq;
    using Common;
    using Domain.CommonModule;
    using Domain.OrganizationModule;
    using global::AutoMapper;
    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    /// <summary>Handler for request to add <see cref="Phone" /> to <see cref="Organization" />.</summary>
    public class AddPhoneToOrganizationRequestHandler :
        ServiceRequestHandler<AddDtoRequest<OrganizationPhoneDto>, AddDtoResponse<OrganizationPhoneDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;
        private readonly IOrganizationRepository _organizationRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddPhoneToOrganizationRequestHandler" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        public AddPhoneToOrganizationRequestHandler ( IOrganizationRepository organizationRepository, ILookupProvider lookupProvider )
        {
            _organizationRepository = organizationRepository;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( AddDtoRequest<OrganizationPhoneDto> request, AddDtoResponse<OrganizationPhoneDto> response )
        {
            var organization = _organizationRepository.GetByKey ( request.AggregateKey );
            var originalPhone = organization.OrganizationPhones.FirstOrDefault ( p => p.GetHashCode () == request.DataTransferObject.OriginalHash );
            var organizationPhoneType = _lookupProvider.Find<OrganizationPhoneType> ( request.DataTransferObject.OrganizationPhoneType.Code );
            var phone = new Phone ( request.DataTransferObject.Phone.Number, request.DataTransferObject.Phone.Extension );

            var organizationPhone = new OrganizationPhone ( organizationPhoneType, phone, request.DataTransferObject.IsPrimary );
            if ( originalPhone != organizationPhone )
            {
                if ( originalPhone != null )
                {
                    organization.RemovePhone ( originalPhone );
                }
                organization.AddPhone ( organizationPhone );
            }
            else if ( organizationPhone.IsPrimary )
            {
                organization.MakePrimary ( organizationPhone );
            }

            response.AggregateKey = organization.Key;
            response.DataTransferObject = Mapper.Map<OrganizationPhone, OrganizationPhoneDto> ( organizationPhone );
            response.DataTransferObject.Key = organization.Key;
        }

        #endregion
    }
}