using SantanderGlobalTech.HackerNews.Domain.Entities;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Domain.Responses
{
    public class OrderStoriesResponse
    {
        /// <summary>
        /// Ordered Story collection
        /// </summary>
        public IEnumerable<Story> OrderedStories { get; set; }
    }
}
