using FluentValidation;
using SantanderGlobalTech.HackerNews.Domain.Requests;

namespace SantanderGlobalTech.HackerNews.Application.Validators.Story
{
    public class ListBestStoriesRequestValidator : BaseValidator<ListBestStoriesRequest>
    {
        public ListBestStoriesRequestValidator()
        {
            RuleFor(request => request).NotNull();
            RuleFor(request => request.Limit).GreaterThan(0).When(request => request.Limit.HasValue);
            RuleFor(request => request.Order).IsInEnum().When(request => request.Order.HasValue);
        }
    }
}
