using Moq;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Mock.Infra.Data.Repositories
{
    public class HackerNewsRepositoryMock : BaseRepositoryMock<IHackerNewsRepository>
    {
        public HackerNewsRepositoryMock SetupGetItemAsync<TItem>(TItem itemToReturn) where TItem : class
        {
            mock.Setup(repository => repository.GetItemAsync<TItem>(It.IsAny<uint>())).ReturnsAsync(itemToReturn);
            return this;
        }

        public HackerNewsRepositoryMock ListBestStoriesIdAsync(IEnumerable<uint> collectionToReturn)
        {
            mock.Setup(repository => repository.ListBestStoriesIdAsync(It.IsAny<int?>())).ReturnsAsync(collectionToReturn);
            return this;
        }
    }
}
