using Surging.Core.ProxyGenerator.Interceptors;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Surging.Core.System.Intercept
{
    public class LogProviderInterceptor : IInterceptor
    {
        public async Task Intercept(IInvocation invocation)
        {
            var watch = Stopwatch.StartNew();
            await invocation.Proceed();
            var result = invocation.ReturnValue;
        }
    }
}