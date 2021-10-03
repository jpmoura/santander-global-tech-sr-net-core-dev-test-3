using SantanderGlobalTech.HackerNews.Domain.Entities;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Domain.Requests
{
    public class OrderStoriesRequest
    {
        /// <summary>
        /// Ordination
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Story collection that will be ordered
        /// </summary>
        public IEnumerable<Story> Stories { get; set; }
    }
}
