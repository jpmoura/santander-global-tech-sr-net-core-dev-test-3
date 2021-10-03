using FluentValidation;
using SantanderGlobalTech.HackerNews.Domain.Requests;

namespace SantanderGlobalTech.HackerNews.Application.Validators.Story
{
    public class OrderStoriesRequestValidator : BaseValidator<OrderStoriesRequest>
    {
        public OrderStoriesRequestValidator()
        {
            RuleFor(request => request).NotNull();
            RuleFor(request => request.Order).NotNull().IsInEnum();
            RuleFor(request => request.Stories).NotNull();
        }
    }
}
