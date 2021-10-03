using Bogus;
using SantanderGlobalTech.HackerNews.Domain.Entities;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Mock.Domain.Entities
{
    public static class StoryMockFactory
    {
        private static readonly Faker<Story> factory = new Faker<Story>().RuleFor(s => s.By, f => f.Internet.UserName())
                                                                         .RuleFor(s => s.Kids, f => new List<int> { f.Random.Int() })
                                                                         .RuleFor(s => s.Score, f => f.Random.Int(0))
                                                                         .RuleFor(s => s.Time, f => f.Date.PastOffset().ToUnixTimeSeconds())
                                                                         .RuleFor(s => s.Title, f => f.Lorem.Sentence())
                                                                         .RuleFor(s => s.Url, f => f.Internet.Url());

        public static Story Create()
        {
            return factory.Generate();
        }
    }
}
