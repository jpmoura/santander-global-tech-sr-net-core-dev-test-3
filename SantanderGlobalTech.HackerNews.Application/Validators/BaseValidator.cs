using FluentValidation;
using FluentValidation.Results;

namespace SantanderGlobalTech.HackerNews.Application.Validators
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel> where TModel : class
    {
        public override ValidationResult Validate(ValidationContext<TModel> context)
        {
            if (context.InstanceToValidate is null)
            {
                return new ValidationResult(new[] { new ValidationFailure("Instance", "Instance cannot be null") });
            }

            return base.Validate(context);
        }
    }
}
