using SantanderGlobalTech.HackerNews.Domain.Enums;

namespace SantanderGlobalTech.HackerNews.Domain.Requests
{
    public class ListBestStoriesRequest
    {
        public int? Limit { get; set; }
        public Order? Order { get; set; }
    }
}
