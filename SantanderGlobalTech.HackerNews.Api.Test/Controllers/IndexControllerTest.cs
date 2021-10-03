using SantanderGlobalTech.HackerNews.Api.DTOs.Story;
using SantanderGlobalTech.HackerNews.Domain.Entities;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using SantanderGlobalTech.HackerNews.Mock.Domain.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SantanderGlobalTech.HackerNews.Api.Test.Controllers
{
    public class IndexControllerTest : BaseTestController
    {
        public IndexControllerTest() : base("/") { }

        [Fact]
        public async Task GivenAListOfBestStoriesOnHackerNews_WhenBest20RouteIsActivated_ThenReturnTheListOfTwentyBestStories()
        {
            IEnumerable<Story> stories = new List<Story> { StoryMockFactory.Create() };
            listBestStoriesUseCaseAsyncMock.SetupExecuteAsync(new ListBestStoriesResponse
            {
                Stories = stories,
            });

            HttpResponseMessage response = await client.GetAsync("best20");
            string content = await response.Content.ReadAsStringAsync();
            IEnumerable<StoryDto> parsedContent = JsonSerializer.Deserialize<IEnumerable<StoryDto>>(content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotEmpty(content);
            Assert.NotEmpty(parsedContent);
        }
    }
}
