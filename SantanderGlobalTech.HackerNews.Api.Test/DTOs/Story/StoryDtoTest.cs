using SantanderGlobalTech.HackerNews.Api.DTOs.Story;
using SantanderGlobalTech.HackerNews.Mock.Domain.Entities;
using System;
using System.Linq;
using Xunit;
using StoryEntity = SantanderGlobalTech.HackerNews.Domain.Entities.Story;

namespace SantanderGlobalTech.HackerNews.Api.Test.DTOs.Story
{
    public class StoryDtoTest
    {
        [Fact]
        public void GivenAStoryEntityToConvertToDto_WhenUseFromMethod_ThenShouldReturnAStoryDto()
        {
            StoryEntity story = StoryMockFactory.Create();

            StoryDto dto = StoryDto.From(story);

            string expectedTime = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            Assert.Equal(story.By, dto.PostedBy);
            Assert.Equal(story.Kids.Count(), dto.CommentCount);
            Assert.Equal(story.Score, dto.Score);
            Assert.Equal(expectedTime, dto.Time);
            Assert.Equal(story.Title, dto.Title);
            Assert.Equal(story.Url, dto.Uri);
        }
    }
}
