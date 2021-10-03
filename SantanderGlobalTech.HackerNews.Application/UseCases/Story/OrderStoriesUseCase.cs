using FluentValidation;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SantanderGlobalTech.HackerNews.Application.UseCases.Story
{
    public class OrderStoriesUseCase : IUseCase<OrderStoriesRequest, OrderStoriesResponse>
    {
        /// <summary>
        /// Use case request validator
        /// </summary>
        private readonly IValidator<OrderStoriesRequest> requestValidator;

        public OrderStoriesUseCase(IValidator<OrderStoriesRequest> requestValidator)
        {
            this.requestValidator = requestValidator;
        }

        /// <summary>
        /// Get a comparable key to be used in LINQ functions
        /// </summary>
        /// <remarks>At this time there is only one prop that can be used but in the future, we have to change this method to add a decision flow about the prop</remarks>
        /// <returns>A selector function</returns>
        private Func<Domain.Entities.Story, IComparable> GetKeySelectorFunc()
        {
            return (Domain.Entities.Story story) => story.Score;
        }

        /// <summary>
        /// Order a story collection by a specific comparable prop
        /// </summary>
        /// <param name="orderBy">Ascending or Descending ordination</param>
        /// <param name="stories">Story collection</param>
        /// <returns>A ordered Story collection</returns>
        private IEnumerable<Domain.Entities.Story> OrderBy(Order orderBy, IEnumerable<Domain.Entities.Story> stories)
        {
            Func<Func<Domain.Entities.Story, IComparable>, IOrderedEnumerable<Domain.Entities.Story>> orderFunction;

            if (orderBy == Order.Asc)
            {
                orderFunction = stories.OrderBy;
            }
            else
            {
                orderFunction = stories.OrderByDescending;
            }

            return orderFunction(GetKeySelectorFunc());
        }

        public OrderStoriesResponse Execute(OrderStoriesRequest request)
        {
            requestValidator.ValidateAndThrow(request);

            return new OrderStoriesResponse
            {
                OrderedStories = OrderBy(request.Order, request.Stories)
            };
        }
    }
}
