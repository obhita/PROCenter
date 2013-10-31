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