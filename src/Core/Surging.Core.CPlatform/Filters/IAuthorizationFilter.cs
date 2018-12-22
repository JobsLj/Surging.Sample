using Surging.Core.CPlatform.Routing;
using System.Threading;

namespace Surging.Core.CPlatform.Filters
{
    public interface IAuthorizationFilter : IFilter
    {
        void ExecuteAuthorizationFilterAsync(ServiceRouteContext serviceRouteContext, CancellationToken cancellationToken);
    }
}