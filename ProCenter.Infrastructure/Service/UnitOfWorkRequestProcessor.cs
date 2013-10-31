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