using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Domain.Contracts.Application
{
    public interface IUseCaseAsync<in TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// Execute a business rule asynchronously
        /// </summary>
        /// <param name="request">Request with all necessary data</param>
        /// <returns>Response in case of success</returns>
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
