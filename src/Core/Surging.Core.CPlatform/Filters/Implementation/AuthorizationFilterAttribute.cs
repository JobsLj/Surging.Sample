using Surging.Core.CPlatform.Messages;
using Surging.Core.CPlatform.Routing;
using System.Threading;

namespace Surging.Core.CPlatform.Filters.Implementation
{
    public abstract class AuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter, IFilter
    {
        public virtual bool OnAuthorization(ServiceRouteContext context)
        {
            return true;
        }

        public virtual void ExecuteAuthorizationFilterAsync(ServiceRouteContext serviceRouteContext, CancellationToken cancellationToken)
        {
            var result = OnAuthorization(serviceRouteContext);
            if (!result)
            {
                serviceRouteContext.ResultMessage.StatusCode = StatusCode.UnAuthentication;
                serviceRouteContext.ResultMessage.ExceptionMessage = "令牌验证失败.";
            }
        }
    }
}