using Bogus;
using SantanderGlobalTech.HackerNews.Domain.Enums;
using System.Collections.Generic;

namespace SantanderGlobalTech.HackerNews.Application.Test.Validators
{
    public static class CommonTestData
    {
        private static readonly Faker faker = new Faker();

        public static IEnumerable<object[]> InvalidOrder => new List<object[]>
        {
            new object[] { (int)Order.Asc - 1 },
            new object[] { (int)Order.Desc + 1 },
        };

        public static IEnumerable<object[]> ValidOrder => new List<object[]>
        {
            new object[] { Order.Asc },
            new object[] { Order.Desc },
        };

        public static IEnumerable<object[]> ValidLimit => new List<object[]>
        {
            new object[] { 1 },
            new object[] { faker.Random.Int(2) },
        };

        public static IEnumerable<object[]> InvalidLimit => new List<object[]>
        {
            new object[] { 0 },
            new object[] { faker.Random.Int(max: -1) },
        };
    }
}
