using System;
using System.Linq;

namespace SantanderGlobalTech.HackerNews.Api.DTOs.Story
{
    /// <summary>
    /// Story entity Data Transfer Object
    /// </summary>
    public class StoryDto
    {
        /// <summary>
        /// The title of the story
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The URL of the story
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The username of the story's author
        /// </summary>
        public string PostedBy { get; set; }

        /// <summary>
        /// Creation date of the story (UTC)
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// The story's score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Number of comments in the story
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="story">Story entity that will be converted into StoryDTO</param>
        public static StoryDto From(Domain.Entities.Story story)
        {
            return new StoryDto
            {
                Title = story.Title,
                Uri = story.Url,
                PostedBy = story.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Score = story.Score,
                CommentCount = story.Kids.Count(),
            };
        }
    }
}
