using FluentValidation.TestHelper;
using SantanderGlobalTech.HackerNews.Application.Validators.Story;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using Xunit;

namespace SantanderGlobalTech.HackerNews.Application.Test.Validators.Story
{
    public class ListBestStoriesRequestValidatorTest
    {
        protected readonly ListBestStoriesRequestValidator sut = new ListBestStoriesRequestValidator();

        public class GivenABestStoriesRequestToValidate : ListBestStoriesRequestValidatorTest
        {
            [Fact]
            public void WhenRequestIsNull_ThenShouldHaveErrorForInstance()
            {
                TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(null);

                result.ShouldHaveValidationErrorFor("Instance");
            }

            public class WhenRequestIsNotNull : GivenABestStoriesRequestToValidate
            {
                public class AndLimit : WhenRequestIsNotNull
                {
                    [Fact]
                    public void IsNotSet_ThenShouldNotHaveValidationErrorForLimitProp()
                    {
                        ListBestStoriesRequest request = new ListBestStoriesRequest();

                        TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                        result.ShouldNotHaveValidationErrorFor(nameof(request.Limit));
                    }

                    public class IsSet : AndLimit
                    {
                        [Theory]
                        [MemberData(nameof(CommonTestData.InvalidLimit), MemberType = typeof(CommonTestData))]
                        public void AndIsInvalid_ThenShouldHaveValidationErrorForLimitProp(int limit)
                        {
                            ListBestStoriesRequest request = new ListBestStoriesRequest
                            {
                                Limit = limit,
                            };

                            TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldHaveValidationErrorFor(nameof(request.Limit));
                        }

                        [Theory]
                        [MemberData(nameof(CommonTestData.ValidLimit), MemberType = typeof(CommonTestData))]
                        public void AndIsValid_ThenShouldNotHaveValidationErrorForLimitProp(int limit)
                        {
                            ListBestStoriesRequest request = new ListBestStoriesRequest
                            {
                                Limit = limit,
                            };

                            TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldNotHaveValidationErrorFor(nameof(request.Limit));
                        }
                    }
                }

                public class AndOrder : WhenRequestIsNotNull
                {
                    [Fact]
                    public void IsNotSet_ThenShouldNotHaveValidationErrorForOrderProp()
                    {
                        ListBestStoriesRequest request = new ListBestStoriesRequest();

                        TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                        result.ShouldNotHaveValidationErrorFor(nameof(request.Order));
                    }

                    public class IsSet : AndOrder
                    {
                        [Theory]
                        [MemberData(nameof(CommonTestData.InvalidOrder), MemberType = typeof(CommonTestData))]
                        public void AndIsInvalid_ThenShouldHaveValidationErrorForOrderProp(int order)
                        {
                            ListBestStoriesRequest request = new ListBestStoriesRequest
                            {
                                Order = (Order)order,
                            };

                            TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldHaveValidationErrorFor(nameof(request.Order));
                        }

                        [Theory]
                        [MemberData(nameof(CommonTestData.ValidOrder), MemberType = typeof(CommonTestData))]
                        public void AndIsValid_ThenShouldNotHaveValidationErrorForOrderProp(Order order)
                        {
                            ListBestStoriesRequest request = new ListBestStoriesRequest
                            {
                                Order = order,
                            };

                            TestValidationResult<ListBestStoriesRequest> result = sut.TestValidate(request);

                            result.ShouldNotHaveValidationErrorFor(nameof(request.Order));
                        }
                    }
                }
            }
        }
    }
}
