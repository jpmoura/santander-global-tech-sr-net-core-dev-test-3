using System.Collections.Generic;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data
{
    public interface IHackerNewsRepository
    {
        /// <summary>
        /// List the IDs of the best stories
        /// </summary>
        /// <param name="limit">If defined, only take the defined number of stories</param>
        /// <returns>A collection of Story IDs</returns>
        Task<IEnumerable<uint>> ListBestStoriesIdAsync(int? limit);

        /// <summary>
        /// Fetch a HackerNews item data
        /// </summary>
        /// <typeparam name="TItem">Item type</typeparam>
        /// <param name="itemId">Item ID</param>
        /// <returns>The Item</returns>
        Task<TItem> GetItemAsync<TItem>(uint itemId);
    }
}
