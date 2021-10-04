using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SantanderGlobalTech.HackerNews.Infra.Data.Http
{
    public class PolicyHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Policies.PolicyStrategy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }
}
