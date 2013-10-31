using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Infrastructure.Service
{
    using Agatha.Common;
    using NLog;
    using Pillar.Domain.Event;
    using Pillar.Domain.FluentRuleEngine.Event;
    using Pillar.FluentRuleEngine;
    using ProCenter.Service.Message.Common;

    /// <summary>
    /// This class is used to handle RuleViolationEvent and map failures to the data transfer object.
    ///   <remarks>
    /// This only works when the Respone is IDtoResponse.
    ///   </remarks>
    /// </summary>
    public class RuleViolationEventInterceptor : IRequestHandlerInterceptor
    {
        #region Constants and Fields

        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<RuleViolation> _validationFailures = new List<RuleViolation>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes after the handling request.
        /// </summary>
        /// <param name="context">
        /// The context. 
        /// </param>
        public void AfterHandlingRequest(RequestProcessingContext context)
        {
            if (context != null && context.Response != null)
            {
                try
                {
                    if (context.Response.Exception == null)
                    {
                        if (_validationFailures.Count > 0)
                        {
                            if (context.Response is IDtoResponse)
                            {
                                var dto = (context.Response as IDtoResponse).GetDto();
                                MapFailures(_validationFailures, dto);
                            }
                            else
                            {
                                Logger.Error(
                                    "Validation failed but no failures mapped to data transfer object, because the response type ({0}) is not derived from {1}",
                                    context.Response.GetType().Name,
                                    typeof(IDtoResponse).Name);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var response = context.Response;
                    response.Exception = new ExceptionInfo(e);
                    response.ExceptionType = ExceptionType.Unknown;
                }
            }
        }

        /// <summary>
        /// Executes before the handling request.
        /// </summary>
        /// <param name="context">
        /// The context. 
        /// </param>
        public void BeforeHandlingRequest(RequestProcessingContext context)
        {
            DomainEvent.Register<RuleViolationEvent>(failure => _validationFailures.AddRange(failure.RuleViolations));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region Methods

        private static void MapFailures(IEnumerable<RuleViolation> validationFailures, IValidatedObject validatedObject)
        {
            foreach (var validationFailure in validationFailures)
            {
                validatedObject.AddDataErrorInfo(
                    new DataErrorInfo(
                        validationFailure.Message,
                        ErrorLevel.Error,
                        PropertyNameMapper.GetDestinationPropertyNames(validationFailure.OffendingObject.GetType(), validatedObject.GetType(), validationFailure.PropertyNames)));
            }
        }

        #endregion
    }
}
