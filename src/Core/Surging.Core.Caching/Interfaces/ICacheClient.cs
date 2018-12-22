using Surging.Core.CPlatform.Cache;
using System.Threading.Tasks;

namespace Surging.Core.Caching.Interfaces
{
    public interface ICacheClient<T>
    {
        T GetClient(CacheEndpoint info, int connectTimeout);

        Task<bool> ConnectionAsync(CacheEndpoint endpoint, int connectTimeout);
    }
}