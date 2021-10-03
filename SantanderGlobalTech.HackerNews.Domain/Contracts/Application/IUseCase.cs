namespace SantanderGlobalTech.HackerNews.Domain.Contracts.Application
{
    public interface IUseCase<in TRequest, out TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// Execute a business rule synchronously
        /// </summary>
        /// <param name="request">Request with all necessary data</param>
        /// <returns>Response in case of success</returns>
        TResponse Execute(TRequest request);
    }
}
