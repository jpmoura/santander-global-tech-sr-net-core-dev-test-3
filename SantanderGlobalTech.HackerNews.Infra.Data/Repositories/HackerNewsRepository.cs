using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Fallback;
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
        /// Fallback policy for Best Stories data fetch
        /// </summary>
        private readonly AsyncFallbackPolicy<IEnumerable<uint>> bestStoriesFallback;

        /// <summary>
        /// Fallback policy for Item data fetch
        /// </summary>
        private readonly AsyncFallbackPolicy<dynamic> itemFallback;

        /// <summary>
        /// Repository logger
        /// </summary>
        private readonly ILogger<HackerNewsRepository> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">App configuration</param>
        /// <param name="cache">App cache</param>
        /// <param name="logger">Repository logger</param>
        public HackerNewsRepository(IConfiguration configuration, IMemoryCache cache, ILogger<HackerNewsRepository> logger)
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
            this.logger = logger;

            bestStoriesFallback = Policy<IEnumerable<uint>>.Handle<Exception>().FallbackAsync(Enumerable.Empty<uint>(), (stories) => { this.logger.LogWarning(stories.Exception, "Best stories fallback fired"); return Task.CompletedTask; });
            itemFallback = Policy<object>.Handle<Exception>().FallbackAsync((object)null, (item) => { this.logger.LogWarning(item.Exception, "Best stories fallback fired"); return Task.CompletedTask; });
        }

        /// <summary>
        /// List the best stories from HackerNews
        /// </summary>
        /// <param name="limit">Set the number of stories that will be returned if not null</param>
        /// <returns>A collection of IDs of the best stories</returns>
        public async Task<IEnumerable<uint>> ListBestStoriesIdAsync(int? limit)
        {
            IEnumerable<uint> bestStories = await bestStoriesFallback.ExecuteAsync(() => bestStoriesUrl.GetJsonAsync<IEnumerable<uint>>());

            if (limit.HasValue)
            {
                return bestStories.Take(limit.Value);
            }

            return bestStories;
        }

        /// <summary>
        /// Get a TItem that is not in Cache and caches it. If Fallback value is provided then its TTL is set to the minimum TTL possible
        /// </summary>
        /// <typeparam name="TItem">Item that will be cached</typeparam>
        /// <param name="cacheEntry">New cache entry</param>
        /// <returns>TItem</returns>
        private async Task<TItem> GetItemWithoutCacheAsync<TItem>(ICacheEntry cacheEntry) where TItem : class
        {
            dynamic item = await itemFallback.ExecuteAsync(() => string.Format(getItemUrl, cacheEntry.Key).GetJsonAsync<dynamic>());
            TimeSpan ttl = item == null ? TimeSpan.FromMilliseconds(1) : TimeSpan.FromMinutes(cacheTtlInMinutes);
            cacheEntry.AbsoluteExpirationRelativeToNow = ttl;
            return ((JObject)item)?.ToObject<TItem>();
        }

        /// <summary>
        /// Get a TItem from HackerNews API
        /// </summary>
        /// <typeparam name="TItem">TItem that will be serialized the response</typeparam>
        /// <param name="itemId">TItem ID</param>
        /// <returns>The TItem found</returns>
        public Task<TItem> GetItemAsync<TItem>(uint itemId) where TItem : class
        {
            return cache.GetOrCreateAsync(itemId, (entry) => GetItemWithoutCacheAsync<TItem>(entry));
        }
    }
}
