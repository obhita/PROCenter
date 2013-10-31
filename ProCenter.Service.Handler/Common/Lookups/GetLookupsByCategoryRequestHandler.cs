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
namespace ProCenter.Service.Handler.Common.Lookups
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Linq;
    using Domain.CommonModule;
    using Domain.CommonModule.Lookups;
    using Service.Message.Common.Lookups;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Handler for getting lookups by category.
    /// </summary>
    public class GetLookupsByCategoryRequestHandler :
        ServiceRequestHandler<GetLookupsByCategoryRequest, GetLookupsByCategoryResponse>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetLookupsByCategoryRequestHandler" /> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public GetLookupsByCategoryRequestHandler ( ILookupProvider lookupProvider )
        {
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetLookupsByCategoryRequest request, GetLookupsByCategoryResponse response )
        {
            var lookups = _lookupProvider.GetAll ( request.Category ).ToList ();
            var lookupDtos = Mapper.Map<List<Lookup>, List<LookupDto>> ( lookups );
            response.Lookups = lookupDtos;
            response.Category = request.Category;
        }

        #endregion
    }
}