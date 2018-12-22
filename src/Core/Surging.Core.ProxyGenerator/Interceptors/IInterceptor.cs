using System.Threading.Tasks;

namespace Surging.Core.ProxyGenerator.Interceptors
{
    public interface IInterceptor
    {
        Task Intercept(IInvocation invocation);
    }
}