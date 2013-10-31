namespace ProCenter.Mvc.Infrastructure.Filter
{
    #region

    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using NLog;

    #endregion

    public class ExtendedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            if (httpActionExecutedContext.Response == null)
            {
                httpActionExecutedContext.Response = new HttpResponseMessage();
            }

            _logger.Error(httpActionExecutedContext.Exception.Message, httpActionExecutedContext.Exception);
            httpActionExecutedContext.Response.StatusCode = HttpStatusCode.InternalServerError;
            httpActionExecutedContext.Response.Content = new StringContent("An error occurred while processing your request.");
            base.OnException(httpActionExecutedContext);
        }
    }
}