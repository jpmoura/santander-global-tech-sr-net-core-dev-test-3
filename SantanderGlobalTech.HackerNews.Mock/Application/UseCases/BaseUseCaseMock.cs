using Moq;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using System;

namespace SantanderGlobalTech.HackerNews.Mock.UseCases
{
    public abstract class BaseUseCaseMock<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        protected readonly Mock<IUseCase<TRequest, TResponse>> mock = new Mock<IUseCase<TRequest, TResponse>>();

        public Mock<IUseCase<TRequest, TResponse>> Mock()
        {
            return mock;
        }

        public IUseCase<TRequest, TResponse> Instance()
        {
            return mock.Object;
        }

        public BaseUseCaseMock<TRequest, TResponse> SetupExecute(TResponse response)
        {
            mock.Setup(useCase => useCase.Execute(It.IsAny<TRequest>())).Returns(response);
            return this;
        }

        public BaseUseCaseMock<TRequest, TResponse> SetupThrows(Exception exception)
        {
            mock.Setup(useCase => useCase.Execute(It.IsAny<TRequest>())).Throws(exception);
            return this;
        }
    }
}
