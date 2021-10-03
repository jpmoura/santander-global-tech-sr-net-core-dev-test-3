using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SantanderGlobalTech.HackerNews.Mock.UseCases.Story;
using System.Net.Http;

namespace SantanderGlobalTech.HackerNews.Api.Test.Controllers
{
    public abstract class BaseTestController
    {
        /// <summary>
        /// In Memory Test Server
        /// </summary>
        protected readonly TestServer server;

        /// <summary>
        /// HTTP Client bound to In Memory HTTP Server
        /// </summary>
        protected readonly HttpClient client;

        /// <summary>
        /// Controller endpoint
        /// </summary>
        protected readonly string endpoint;

        #region Mocks
        protected readonly ListBestStoriesUseCaseAsyncMock listBestStoriesUseCaseAsyncMock = new ListBestStoriesUseCaseAsyncMock();
        #endregion

        protected BaseTestController(string endpoint)
        {
            server = new TestServer(new WebHostBuilder().UseEnvironment("unit-testing").UseStartup<Startup>().ConfigureTestServices(services =>
            {
                AddTestServices(services);
            }));

            client = server.CreateClient();
            this.endpoint = endpoint;
        }

        /// <summary>
        /// Add all mocked services
        /// </summary>
        /// <param name="services">Webhost service collection</param>
        private void AddTestServices(IServiceCollection services)
        {
            services.AddSingleton(listBestStoriesUseCaseAsyncMock.Instance());
        }
    }
}
