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
namespace ProCenter.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Agatha.Common;
    using Agatha.Common.Caching;
    using Agatha.ServiceLayer;
    using Pillar.Domain.Event;
    using Pillar.Domain.FluentRuleEngine.Event;

    #endregion

    /// <summary>
    ///     Request processor that manages the unit of work.
    /// </summary>
    public class UnitOfWorkRequestProcessor : RequestProcessorBase
    {
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private bool _validationFailureOccurred;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkRequestProcessor" /> class.
        /// </summary>
        /// <param name="serviceLayerConfiguration">The service layer configuration.</param>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public UnitOfWorkRequestProcessor (
            ServiceLayerConfiguration serviceLayerConfiguration,
            ICacheManager cacheManager,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base ( serviceLayerConfiguration, cacheManager )
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        #endregion

        #region Methods

        protected override void BeforeProcessing(IEnumerable<Request> requests)
        {
            DomainEvent.Register<RuleViolationEvent>(failure => { _validationFailureOccurred = true; });
            base.BeforeProcessing(requests);
        }

        /// <summary>
        ///     Afters the processing.
        /// </summary>
        /// <param name="requests">The requests.</param>
        /// <param name="responses">The responses.</param>
        protected override void AfterProcessing ( IEnumerable<Request> requests, IEnumerable<Response> responses )
        {
            responses = responses.ToList ();
            base.AfterProcessing ( requests, responses );
            if ( !_validationFailureOccurred )
            {
                var unitOfWork = _unitOfWorkProvider.GetCurrentUnitOfWork ();

                unitOfWork.Commit ();
            }
        }

        /// <summary>
        ///     Disposes the unmanaged resources.
        /// </summary>
        protected override void DisposeUnmanagedResources ()
        {
            base.DisposeUnmanagedResources ();

            var unitOfWork = _unitOfWorkProvider.GetCurrentUnitOfWork();

            if ( unitOfWork is IDisposable )
            {
                ( unitOfWork as IDisposable ).Dispose ();
            }
        }

        #endregion
    }
}