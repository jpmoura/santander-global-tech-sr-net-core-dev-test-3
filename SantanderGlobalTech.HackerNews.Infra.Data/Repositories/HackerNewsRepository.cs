using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Infra.Data.Repositories
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        /// <summary>
        /// HackerNews Best Stories endpoint
        /// </summary>
        private readonly Url bestStoriesUrl;

        /// <summary>
        /// HackerNews Item endpoint
        /// </summary>
        private readonly string getItemUrl;

        /// <summary>
        /// Application cache
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// Application Cache Time To Live
        /// </summary>
        private readonly int cacheTtlInMinutes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">App configuration</param>
        /// <param name="cache">App cache</param>
        public HackerNewsRepository(IConfiguration configuration, IMemoryCache cache)
        {
            bestStoriesUrl = configuration["HackerNews:BestStoriesUrl"];
            getItemUrl = configuration["HackerNews:GetItemUrl"];
            bool wasParsed = int.TryParse(configuration["HackerNews:CacheTtlInMinutes"], out int cacheTtlInMinutes);

            if (!wasParsed)
            {
                cacheTtlInMinutes = 5;
            }

            this.cacheTtlInMinutes = cacheTtlInMinutes;
            this.cache = cache;
        }

        /// <summary>
        /// List the best stories from HackerNews
        /// </summary>
        /// <param name="limit">Set the number of stories that will be returned if not null</param>
        /// <returns>A collection of IDs of the best stories</returns>
        public async Task<IEnumerable<uint>> ListBestStoriesIdAsync(int? limit)
        {
            IEnumerable<uint> bestStories = await bestStoriesUrl.GetJsonAsync<IEnumerable<uint>>();

            if (limit.HasValue)
            {
                return bestStories.Take(limit.Value);
            }

            return bestStories;
        }

        /// <summary>
        /// Get a TItem that is not in Cache ans cache it
        /// </summary>
        /// <typeparam name="TItem">Item that will be cached</typeparam>
        /// <param name="cacheEntry">New cache entry</param>
        /// <returns>TItem</returns>
        private Task<TItem> GetItemWithoutCacheAsync<TItem>(ICacheEntry cacheEntry)
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTtlInMinutes);
            return string.Format(getItemUrl, cacheEntry.Key).GetJsonAsync<TItem>();
        }

        /// <summary>
        /// Get a TItem from HackerNews API
        /// </summary>
        /// <typeparam name="TItem">TItem that will be serialized the response</typeparam>
        /// <param name="itemId">TItem ID</param>
        /// <returns>The TItem found</returns>
        public Task<TItem> GetItemAsync<TItem>(uint itemId)
        {
            return cache.GetOrCreateAsync(itemId, (entry) => GetItemWithoutCacheAsync<TItem>(entry));
        }
    }
}
