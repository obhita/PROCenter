namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Threading.Tasks;
    using Agatha.Common;

    #endregion

    public interface IAsyncRequestDispatcher : IRequestDispatcher
    {
        Task<T> GetAsync<T>()
            where T : Response;

        Task<object> GetAsync(Type type);

        Task GetAllAsync();
    }
}