using FluentValidation;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using SantanderGlobalTech.HackerNews.Application.UseCases.Story;
using SantanderGlobalTech.HackerNews.Application.Validators.Story;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Application;
using SantanderGlobalTech.HackerNews.Domain.Contracts.Infra.Data;
using SantanderGlobalTech.HackerNews.Domain.Requests;
using SantanderGlobalTech.HackerNews.Domain.Responses;
using SantanderGlobalTech.HackerNews.Infra.Data.Http;
using SantanderGlobalTech.HackerNews.Infra.Data.Repositories;

namespace SantanderGlobalTech.HackerNews.Infra.IoC
{
    public static class IocInstallerExtension
    {
        /// <summary>
        /// Adds all repositories in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with repositories added</returns>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IHackerNewsRepository, HackerNewsRepository>();

            return services;
        }

        /// <summary>
        /// Adds all HTTP related services in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with HTTP services added</returns>
        private static IServiceCollection AddHttp(this IServiceCollection services)
        {
            FlurlHttp.Configure(settings => settings.HttpClientFactory = new PollyHttpClientFactory());

            return services;
        }

        /// <summary>
        /// Adds all use cases in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with use cases added</returns>
        private static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddSingleton<IUseCaseAsync<ListBestStoriesRequest, ListBestStoriesResponse>, ListBestStoriesUseCaseAsync>();
            services.AddSingleton<IUseCase<OrderStoriesRequest, OrderStoriesResponse>, OrderStoriesUseCase>();

            return services;
        }

        /// <summary>
        /// Adds all validators in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with validators added</returns>
        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<ListBestStoriesRequest>, ListBestStoriesRequestValidator>();
            services.AddSingleton<IValidator<OrderStoriesRequest>, OrderStoriesRequestValidator>();

            return services;
        }

        /// <summary>
        /// Adds all application layer services in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with application services added</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddUseCases();
            services.AddValidators();

            return services;
        }

        /// <summary>
        /// Adds all infrastructure layer services in service collection
        /// </summary>
        /// <param name="services">Current service collection</param>
        /// <returns>Service collection with infrastructure services added</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddHttp();

            return services;
        }
    }
}
