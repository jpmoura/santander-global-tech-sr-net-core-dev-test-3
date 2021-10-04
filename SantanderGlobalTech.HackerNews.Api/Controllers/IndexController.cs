using Microsoft.AspNetCore.Mvc;
using SantanderGlobalTech.HackerNews.Api.DTOs.Story;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Api.Controllers
{
    /// <summary>
    /// Index Controller
    /// </summary>
    [ApiController]
    [Route("")]
    [Produces("application/json")]
    public class IndexController : Controller
    {
        /// <summary>
        /// Use case responsible to list the best stories of HackerNews
        /// </summary>
        private readonly IUseCaseAsync<ListBestStoriesRequest, ListBestStoriesResponse> listStoriesUseCaseAsync;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listStoriesUseCaseAsync">Use case that will list the best stories on HackerNews</param>
        public IndexController(IUseCaseAsync<ListBestStoriesRequest, ListBestStoriesResponse> listStoriesUseCaseAsync)
        {
            this.listStoriesUseCaseAsync = listStoriesUseCaseAsync;
        }

        /// <summary>
        /// Get the 20 best stories in HackerNews
        /// </summary>
        /// <returns>A collection of the best 20 stories in HackerNews</returns>
        [Route("best20")]
        [HttpGet]
        public async Task<IAsyncEnumerable<StoryDto>> GetStories()
        {
            ListBestStoriesRequest request = new ListBestStoriesRequest
            {
                Limit = 20,
                Order = Order.Desc,
            };

            ListBestStoriesResponse response = await listStoriesUseCaseAsync.ExecuteAsync(request);

            return response.Stories.Select((story) => StoryDto.From(story)).ToAsyncEnumerable();
        }
    }
}
