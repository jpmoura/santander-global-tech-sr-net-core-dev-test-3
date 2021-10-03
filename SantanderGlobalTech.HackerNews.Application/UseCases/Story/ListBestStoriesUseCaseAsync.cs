using FluentValidation;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Application.UseCases.Story
{
    public class ListBestStoriesUseCaseAsync : IUseCaseAsync<ListBestStoriesRequest, ListBestStoriesResponse>
    {
        /// <summary>
        /// HackerNews Repository
        /// </summary>
        private readonly IHackerNewsRepository hackerNewsRepository;

        /// <summary>
        /// Request validator
        /// </summary>
        private readonly IValidator<ListBestStoriesRequest> requestValidator;

        private readonly IUseCase<OrderStoriesRequest, OrderStoriesResponse> orderStoriesUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hackerNewsRepository">HackerNews Repository</param>
        public ListBestStoriesUseCaseAsync(IHackerNewsRepository hackerNewsRepository,
                                           IValidator<ListBestStoriesRequest> requestValidator,
                                           IUseCase<OrderStoriesRequest, OrderStoriesResponse> orderStoriesUseCase)
        {
            this.hackerNewsRepository = hackerNewsRepository;
            this.requestValidator = requestValidator;
            this.orderStoriesUseCase = orderStoriesUseCase;
        }

        /// <summary>
        /// Get a single Story
        /// </summary>
        /// <param name="storyId">Story ID</param>
        /// <param name="stories">Story collection to store the story after fetch it</param>
        /// <returns>Void</returns>
        private async Task GetStoryAsync(uint storyId, ConcurrentBag<Domain.Entities.Story> stories)
        {
            Domain.Entities.Story story = await hackerNewsRepository.GetItemAsync<Domain.Entities.Story>(storyId);

            if (story is null)
            {
                return;
            }

            stories.Add(story);
        }

        /// <summary>
        /// Fetch a collection of Story based on an ID list
        /// </summary>
        /// <param name="storiesId">Stories ID</param>
        /// <returns>A collection of Story</returns>
        private async Task<IEnumerable<Domain.Entities.Story>> GetStoriesAsync(IEnumerable<uint> storiesId)
        {
            ConcurrentBag<Domain.Entities.Story> stories = new ConcurrentBag<Domain.Entities.Story>();

            await Task.WhenAll(storiesId.Select((storyId) => GetStoryAsync(storyId, stories)));

            return stories;
        }

        /// <summary>
        /// Fetch the best stories from HackerNews
        /// </summary>
        /// <returns>An ordered collection of the best stories</returns>
        public async Task<ListBestStoriesResponse> ExecuteAsync(ListBestStoriesRequest request)
        {
            requestValidator.ValidateAndThrow(request);

            IEnumerable<uint> bestStoriesId = await hackerNewsRepository.ListBestStoriesIdAsync(request.Limit);
            IEnumerable<Domain.Entities.Story> bestStories = await GetStoriesAsync(bestStoriesId);

            if (request.Order.HasValue)
            {
                bestStories = orderStoriesUseCase.Execute(new OrderStoriesRequest
                {
                    Order = request.Order.Value,
                    Stories = bestStories
                }).OrderedStories;
            }

            return new ListBestStoriesResponse
            {
                Stories = bestStories,
            };
        }
    }
}
