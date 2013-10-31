namespace ProCenter.Mvc.Controllers.Api
{
    using System.Web.Http;
    using Agatha.Common;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    public class BaseApiController : ApiController
    {
        private readonly IRequestDispatcherFactory _requestDispatcherFactory;

        protected BaseApiController()
        {
        }

        protected BaseApiController(IRequestDispatcherFactory requestDispatcherFactory)
        {
            _requestDispatcherFactory = requestDispatcherFactory;
        }

        public IAsyncRequestDispatcher CreateAsyncRequestDispatcher()
        {
            return _requestDispatcherFactory.CreateRequestDispatcher() as IAsyncRequestDispatcher;
        }
    }
}