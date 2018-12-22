using Surging.Core.Caching.HashAlgorithms;
using System.Threading.Tasks;

namespace Surging.Core.Caching.AddressResolvers
{
    public interface IAddressResolver
    {
        ValueTask<ConsistentHashNode> Resolver(string cacheId, string item);
    }
}