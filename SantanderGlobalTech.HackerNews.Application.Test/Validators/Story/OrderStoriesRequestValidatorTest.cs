using FluentValidation.TestHelper;
using SantanderGlobalTech.HackerNews.Application.Validators.Story;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using System.Collections.Generic;
using Xunit;
using StoryEntity = SantanderGlobalTech.HackerNews.Domain.Entities.Story;

namespace SantanderGlobalTech.HackerNews.Application.Test.Validators.Story
{
    public class OrderStoriesRequestValidatorTest
    {
        protected OrderStoriesRequestValidator sut = new OrderStoriesRequestValidator();

        public class GivenAStoryCollectionToOrder : OrderStoriesRequestValidatorTest
        {
            [Fact]
            public void WhenRequestIsNull_ThenShouldHaveErrorForInstance()
            {
                TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(null);

                result.ShouldHaveValidationErrorFor("Instance");
            }

            public class WhenRequestIsNotNull : GivenAStoryCollectionToOrder
            {
                public class AndOrder : WhenRequestIsNotNull
                {
                    public void IsNotSet_ThenShouldHaveValidationErrorForOrderProp()
                    {
                        OrderStoriesRequest request = new OrderStoriesRequest();

                        TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(request);

                        result.ShouldHaveValidationErrorFor(nameof(request.Order));
                    }

                    public class IsSet : AndOrder
                    {
                        [Theory]
                        [MemberData(nameof(CommonTestData.InvalidOrder), MemberType = typeof(CommonTestData))]
                        public void AndIsInvalid_ThenShouldHaveValidationErrorForOrderProp(int order)
                        {
                            OrderStoriesRequest request = new OrderStoriesRequest
                            {
                                Order = (Order)order,
                            };

                            TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldHaveValidationErrorFor(nameof(request.Order));
                        }

                        [Theory]
                        [MemberData(nameof(CommonTestData.ValidOrder), MemberType = typeof(CommonTestData))]
                        public void AndIsValid_ThenShouldNotHaveValidationErrorForOrderProp(Order order)
                        {
                            OrderStoriesRequest request = new OrderStoriesRequest
                            {
                                Order = order,
                            };

                            TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldNotHaveValidationErrorFor(nameof(request.Order));
                        }
                    }
                }

                public class AndStories : GivenAStoryCollectionToOrder
                {
                    [Fact]
                    public void IsNotSet_ThenShouldHaveValidationErrorForStoriesProps()
                    {
                        OrderStoriesRequest request = new OrderStoriesRequest();

                        TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(request);

                        result.ShouldHaveValidationErrorFor(nameof(request.Stories));
                    }

                    [Fact]
                    public void IsSet_ThenShouldNotHaveValidationErrorForStoriesProps()
                    {
                        OrderStoriesRequest request = new OrderStoriesRequest
                        {
                            Stories = new List<StoryEntity>(),
                        };

                        TestValidationResult<OrderStoriesRequest> result = sut.TestValidate(request);

                        result.ShouldNotHaveValidationErrorFor(nameof(request.Stories));
                    }
                }
            }
        }
    }
}
