using FluentValidation;
using Moq;
using System;

namespace SantanderGlobalTech.HackerNews.Mock.Validators
{
    public abstract class BaseValidatorMock<TRequest> where TRequest : class
    {
        protected readonly Mock<IValidator<TRequest>> mock = new Mock<IValidator<TRequest>>();

        public Mock<IValidator<TRequest>> Mock()
        {
            return mock;
        }

        public IValidator<TRequest> Instance()
        {
            return mock.Object;
        }

        public BaseValidatorMock<TRequest> SetupThrows(Exception exception)
        {
            mock.Setup(validator => validator.Validate(It.IsAny<ValidationContext<TRequest>>())).Throws(exception);
            return this;
        }
    }
}
