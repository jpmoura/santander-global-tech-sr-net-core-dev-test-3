using Flurl.Http.Configuration;
using System.Net.Http;

namespace SantanderGlobalTech.HackerNews.Infra.Data.Http
{
    public class PollyHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new PolicyHandler
            {
                InnerHandler = base.CreateMessageHandler()
            };
        }
    }
}
