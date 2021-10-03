using Bogus;
using FluentValidation;
using Moq;
using SantanderGlobalTech.HackerNews.Application.UseCases.Story;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using SantanderGlobalTech.HackerNews.Mock.Validators.Story;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;
using StoryEntity = SantanderGlobalTech.HackerNews.Domain.Entities.Story;

namespace SantanderGlobalTech.HackerNews.Application.Test.UseCases
{
    public class OrderStoriesUseCaseTest
    {
        protected readonly Faker faker = new Faker();

        public class GivenACollectionToOrder : OrderStoriesUseCaseTest
        {
            protected OrderStoriesUseCase CreateSut(IValidator<OrderStoriesRequest> requestValidatorMock)
            {
                return new OrderStoriesUseCase(requestValidatorMock);
            }

            public class WhenRequestIsInvalid : GivenACollectionToOrder
            {
                [Fact]
                public void ThenThrowsValidationException()
                {
                    OrderStoriesRequestValidatorMock validatorMock = new OrderStoriesRequestValidatorMock();
                    validatorMock.SetupThrows(new ValidationException(faker.Lorem.Sentence()));
                    OrderStoriesUseCase sut = CreateSut(validatorMock.Instance());

                    Assert.Throws<ValidationException>(() => sut.Execute(null));
                    validatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<OrderStoriesRequest>>()), Times.Once);
                }
            }

            public class WhenRequestIsValid : GivenACollectionToOrder
            {
                public class AndOrderIsAsc : WhenRequestIsValid
                {
                    [Fact]
                    public void ThenShouldOrderByScorePropAscending()
                    {
                        OrderStoriesRequestValidatorMock validatorMock = new OrderStoriesRequestValidatorMock();
                        OrderStoriesUseCase sut = CreateSut(validatorMock.Instance());
                        OrderStoriesRequest request = new OrderStoriesRequest
                        {
                            Order = Order.Asc,
                            Stories = new List<StoryEntity> { new StoryEntity { Score = 2 }, new StoryEntity { Score = 1 } },
                        };

                        OrderStoriesResponse response = sut.Execute(request);

                        validatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<OrderStoriesRequest>>()), Times.Once);
                        Assert.NotNull(response);
                        Assert.NotNull(response.OrderedStories);
                        Assert.True(response.OrderedStories.First().Score == 1);
                        Assert.True(response.OrderedStories.Last().Score == 2);
                    }
                }

                public class AndOrderIsDesc : WhenRequestIsValid
                {
                    [Fact]
                    public void ThenShouldOrderByScorePropDescending()
                    {
                        OrderStoriesRequestValidatorMock validatorMock = new OrderStoriesRequestValidatorMock();
                        OrderStoriesUseCase sut = CreateSut(validatorMock.Instance());
                        OrderStoriesRequest request = new OrderStoriesRequest
                        {
                            Order = Order.Desc,
                            Stories = new List<StoryEntity> { new StoryEntity { Score = 1 }, new StoryEntity { Score = 2 } },
                        };

                        OrderStoriesResponse response = sut.Execute(request);

                        validatorMock.Mock().Verify(mock => mock.Validate(It.IsAny<ValidationContext<OrderStoriesRequest>>()), Times.Once);
                        Assert.NotNull(response);
                        Assert.NotNull(response.OrderedStories);
                        Assert.True(response.OrderedStories.First().Score == 2);
                        Assert.True(response.OrderedStories.Last().Score == 1);
                    }
                }
            }
        }
    }
}
