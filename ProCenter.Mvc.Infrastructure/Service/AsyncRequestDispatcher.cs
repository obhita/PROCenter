namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Agatha.Common;
    using Agatha.Common.Caching;

    #endregion

    public class AsyncRequestDispatcher : RequestDispatcher, IAsyncRequestDispatcher
    {
        private readonly HttpContext _httpContext;

        public AsyncRequestDispatcher(IRequestProcessor requestProcessor, ICacheManager cacheManager)
            : base(requestProcessor, cacheManager)
        {
            _httpContext = HttpContext.Current;
        }

        public async Task<T> GetAsync<T>()
            where T : Response
        {
            T response = null;
            await Task.Run(() =>
                {
                    HttpContext.Current = _httpContext;
                    response = base.Get<T>();
                });
            return response;
        }

        public async Task<object> GetAsync(Type type)
        {
            object response = null;
            await Task.Run(() =>
                {
                    HttpContext.Current = _httpContext;
                    response = Responses.First(f => f.GetType() == type);
                });
            return response;
        }

        public Task GetAllAsync()
        {
            return Task.Run(() =>
                {
                    HttpContext.Current = _httpContext;
                    //This Triggers dispatcher to get responses
                    HasResponse<Response>();
                });
        }
    }
}