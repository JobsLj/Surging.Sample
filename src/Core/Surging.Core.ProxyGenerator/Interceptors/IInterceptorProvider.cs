using System;
using System.Collections.Generic;

namespace Surging.Core.ProxyGenerator.Interceptors
{
    public interface IInterceptorProvider
    {
        IInvocation GetInvocation(object proxy, IDictionary<string, object> parameters, string serviceId, Type returnType);

        IInvocation GetCacheInvocation(object proxy, IDictionary<string, object> parameters, string serviceId, Type returnType);
    }
}