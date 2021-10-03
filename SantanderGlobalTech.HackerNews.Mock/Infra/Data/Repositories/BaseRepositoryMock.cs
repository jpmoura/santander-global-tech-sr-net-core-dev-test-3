using Moq;

namespace SantanderGlobalTech.HackerNews.Mock.Infra.Data.Repositories
{
    public abstract class BaseRepositoryMock<TRepository> where TRepository : class
    {
        protected readonly Mock<TRepository> mock = new Mock<TRepository>();

        public Mock<TRepository> Mock()
        {
            return mock;
        }

        public TRepository Instance()
        {
            return mock.Object;
        }
    }
}
