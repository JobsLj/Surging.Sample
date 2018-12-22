using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.KestrelHttpServer.Middlewares
{
    public interface IAuthorizationServerProvider
    {
        Task<bool> ValidateClientAuthentication(string token);

        Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters);

        Task<string> GetPayloadString(string token);
    }
}