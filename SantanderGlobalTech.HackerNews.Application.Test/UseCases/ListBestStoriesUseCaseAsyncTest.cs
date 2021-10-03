using Bogus;
using FluentValidation;
using Moq;
using SantanderGlobalTech.HackerNews.Application.UseCases.Story;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data;
using SantanderGlobalTech.HackerNews.Domain.Entities;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using SantanderGlobalTech.HackerNews.Mock.Domain.Entities;
using SantanderGlobalTech.HackerNews.Mock.Infra.Data.Repositories;
using SantanderGlobalTech.HackerNews.Mock.UseCases.Story;
using SantanderGlobalTech.HackerNews.Mock.Validators.Story;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace SantanderGlobalTech.HackerNews.Application.Test.UseCases
{
    public class ListBestStoriesUseCaseAsyncTest
    {
        protected readonly Faker faker = new Faker();

        protected ListBestStoriesUseCaseAsync CreateSut(IHackerNewsRepository hackerNewsRepositoryMock,
                                                        IValidator<ListBestStoriesRequest> requestValidatorMock,
                                                        IUseCase<OrderStoriesRequest, OrderStoriesResponse> orderStoriesUseCaseMock)
        {
            return new ListBestStoriesUseCaseAsync(hackerNewsRepositoryMock, requestValidatorMock, orderStoriesUseCaseMock);
        }

        public class GivenAListOfBestStories : ListBestStoriesUseCaseAsyncTest
        {
            [Fact]
            public async Task WhenRequestIsInvalid_ThenShouldThrowValidationException()
            {
                HackerNewsRepositoryMock hackerNewsRepositoryMock = new HackerNewsRepositoryMock();
                ListBestStoriesRequestValidatorMock requestValidatorMock = new ListBestStoriesRequestValidatorMock();
                requestValidatorMock.SetupThrows(new ValidationException(faker.Lorem.Sentence()));
                OrderStoriesUseCaseMock orderStoriesUseCaseMock = new OrderStoriesUseCaseMock();
                ListBestStoriesUseCaseAsync sut = CreateSut(hackerNewsRepositoryMock.Instance(), requestValidatorMock.Instance(), orderStoriesUseCaseMock.Instance());

                await Assert.ThrowsAsync<ValidationException>(() => sut.ExecuteAsync(null));
                requestValidatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<ListBestStoriesRequest>>()), Times.Once);
                hackerNewsRepositoryMock.Mock().Verify(mock => mock.ListBestStoriesIdAsync(It.IsAny<int>()), Times.Never);
                hackerNewsRepositoryMock.Mock().Verify(mock => mock.GetItemAsync<Story>(It.IsAny<uint>()), Times.Never);
                orderStoriesUseCaseMock.Mock().Verify(mock => mock.Execute(It.IsAny<OrderStoriesRequest>()), Times.Never);
            }

            public class WhenRequestIsValid : GivenAListOfBestStories
            {
                [Fact]
                public async Task AndStoryIsNotFound_ThenShouldNotIncludeInResponseCollection()
                {
                    HackerNewsRepositoryMock hackerNewsRepositoryMock = new HackerNewsRepositoryMock();
                    List<uint> bestStoriesIdCollectionMock = new List<uint> { faker.Random.UInt() };
                    hackerNewsRepositoryMock.ListBestStoriesIdAsync(bestStoriesIdCollectionMock);
                    ListBestStoriesRequestValidatorMock requestValidatorMock = new ListBestStoriesRequestValidatorMock();
                    OrderStoriesUseCaseMock orderStoriesUseCaseMock = new OrderStoriesUseCaseMock();
                    ListBestStoriesUseCaseAsync sut = CreateSut(hackerNewsRepositoryMock.Instance(), requestValidatorMock.Instance(), orderStoriesUseCaseMock.Instance());

                    ListBestStoriesResponse response = await sut.ExecuteAsync(new ListBestStoriesRequest
                    {
                        Limit = faker.Random.Int(0),
                        Order = null,
                    });

                    Assert.NotNull(response);
                    Assert.NotNull(response.Stories);
                    Assert.Empty(response.Stories);
                    requestValidatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<ListBestStoriesRequest>>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.ListBestStoriesIdAsync(It.IsAny<int>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.GetItemAsync<Story>(It.IsAny<uint>()), Times.Exactly(bestStoriesIdCollectionMock.Count));
                    orderStoriesUseCaseMock.Mock().Verify(mock => mock.Execute(It.IsAny<OrderStoriesRequest>()), Times.Never);
                }

                [Fact]
                public async Task AndOrderIsNotSet_ThenShouldNotInvokeOrderUseCase()
                {
                    HackerNewsRepositoryMock hackerNewsRepositoryMock = new HackerNewsRepositoryMock();
                    List<uint> bestStoriesIdCollectionMock = new List<uint> { faker.Random.UInt(), faker.Random.UInt(), faker.Random.UInt() };
                    hackerNewsRepositoryMock.SetupGetItemAsync(StoryMockFactory.Create());
                    hackerNewsRepositoryMock.ListBestStoriesIdAsync(bestStoriesIdCollectionMock);
                    ListBestStoriesRequestValidatorMock requestValidatorMock = new ListBestStoriesRequestValidatorMock();
                    OrderStoriesUseCaseMock orderStoriesUseCaseMock = new OrderStoriesUseCaseMock();
                    ListBestStoriesUseCaseAsync sut = CreateSut(hackerNewsRepositoryMock.Instance(), requestValidatorMock.Instance(), orderStoriesUseCaseMock.Instance());

                    ListBestStoriesResponse response = await sut.ExecuteAsync(new ListBestStoriesRequest
                    {
                        Limit = faker.Random.Int(0),
                        Order = null,
                    });

                    Assert.NotNull(response);
                    Assert.NotNull(response.Stories);
                    requestValidatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<ListBestStoriesRequest>>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.ListBestStoriesIdAsync(It.IsAny<int>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.GetItemAsync<Story>(It.IsAny<uint>()), Times.Exactly(bestStoriesIdCollectionMock.Count));
                    orderStoriesUseCaseMock.Mock().Verify(mock => mock.Execute(It.IsAny<OrderStoriesRequest>()), Times.Never);
                }

                [Fact]
                public async Task AndOrderIsSet_ThenShouldInvokeOrderUseCase()
                {
                    HackerNewsRepositoryMock hackerNewsRepositoryMock = new HackerNewsRepositoryMock();
                    List<uint> bestStoriesIdCollectionMock = new List<uint> { faker.Random.UInt(), faker.Random.UInt(), faker.Random.UInt() };
                    hackerNewsRepositoryMock.SetupGetItemAsync(StoryMockFactory.Create());
                    hackerNewsRepositoryMock.ListBestStoriesIdAsync(bestStoriesIdCollectionMock);
                    ListBestStoriesRequestValidatorMock requestValidatorMock = new ListBestStoriesRequestValidatorMock();
                    OrderStoriesUseCaseMock orderStoriesUseCaseMock = new OrderStoriesUseCaseMock();
                    orderStoriesUseCaseMock.SetupExecute(new OrderStoriesResponse { OrderedStories = new List<Story> { StoryMockFactory.Create() } });
                    ListBestStoriesUseCaseAsync sut = CreateSut(hackerNewsRepositoryMock.Instance(), requestValidatorMock.Instance(), orderStoriesUseCaseMock.Instance());

                    ListBestStoriesResponse response = await sut.ExecuteAsync(new ListBestStoriesRequest
                    {
                        Limit = faker.Random.Int(0),
                        Order = faker.PickRandom<Order>(),
                    });

                    Assert.NotNull(response);
                    Assert.NotNull(response.Stories);
                    requestValidatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<ListBestStoriesRequest>>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.ListBestStoriesIdAsync(It.IsAny<int>()), Times.Once);
                    hackerNewsRepositoryMock.Mock().Verify(mock => mock.GetItemAsync<Story>(It.IsAny<uint>()), Times.Exactly(bestStoriesIdCollectionMock.Count));
                    orderStoriesUseCaseMock.Mock().Verify(mock => mock.Execute(It.IsAny<OrderStoriesRequest>()), Times.Once);
                }
            }
        }
    }
}
