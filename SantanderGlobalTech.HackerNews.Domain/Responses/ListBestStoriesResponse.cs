using SantanderGlobalTech.HackerNews.Domain.Entities;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Domain.Responses
{
    public class ListBestStoriesResponse
    {
        public IEnumerable<Story> Stories;
    }
}
