using Surging.Core.CPlatform.Filters.Implementation;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Filters
{
    public interface IExceptionFilter : IFilter
    {
        Task ExecuteExceptionFilterAsync(RpcActionExecutedContext actionExecutedContext, CancellationToken cancellationToken);
    }
}