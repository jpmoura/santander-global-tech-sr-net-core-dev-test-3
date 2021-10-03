using Moq;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using System;

namespace SantanderGlobalTech.HackerNews.Mock.UseCases
{
    public abstract class BaseUseCaseAsyncMock<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        protected readonly Mock<IUseCaseAsync<TRequest, TResponse>> mock = new Mock<IUseCaseAsync<TRequest, TResponse>>();

        public Mock<IUseCaseAsync<TRequest, TResponse>> Mock()
        {
            return mock;
        }

        public IUseCaseAsync<TRequest, TResponse> Instance()
        {
            return mock.Object;
        }

        public BaseUseCaseAsyncMock<TRequest, TResponse> SetupExecuteAsync(TResponse response)
        {
            mock.Setup(useCase => useCase.ExecuteAsync(It.IsAny<TRequest>())).ReturnsAsync(response);
            return this;
        }

        public BaseUseCaseAsyncMock<TRequest, TResponse> SetupThrowAsync(Exception exception)
        {
            mock.Setup(useCase => useCase.ExecuteAsync(It.IsAny<TRequest>())).ThrowsAsync(exception);
            return this;
        }
    }
}
