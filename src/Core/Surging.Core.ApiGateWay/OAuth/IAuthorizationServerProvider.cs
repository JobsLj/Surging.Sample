using Surging.Core.System.SystemType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.ApiGateWay.OAuth
{
    public interface IAuthorizationServerProvider
    {
        Task<string> GenerateTokenCredential(Dictionary<string, object> parameters, AccessSystemType accessSystemType = AccessSystemType.Inner);

        Task<bool> ValidateClientAuthentication(string token);

        Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters);

        string GetPayloadString(string token);
    }
}