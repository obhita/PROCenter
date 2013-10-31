using System;
using Agatha.Common;
using Agatha.ServiceLayer;
using NLog;

namespace ProCenter.Service.Handler.Common
{
    public abstract class ServiceRequestHandler<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Response, new()
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override Response Handle(TRequest request)
        {
            var response = CreateTypedResponse();

            try
            {
                Handle(request, response);
            }
            catch (Exception ex)
            {
                logger.Error("\r\n" + ex, GetType().FullName);

                // NOT throw the exception out here which means agatha exception mechanism is ignored
                // so that the ExceptionInfo in agatha response is always null
                throw;
            }

            return response;
        }

        protected abstract void Handle(TRequest request, TResponse response);
    }
}
