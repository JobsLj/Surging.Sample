using Newtonsoft.Json;
using Surging.Core.CPlatform.Transport.Implementation;

namespace Surging.Core.CPlatform.Runtime.Session
{
    public class RpcContextSession : SurgingSessionBase
    {
        private const string PayloadKey = "payload";

        internal RpcContextSession()
        {
        }

        public override string UserId
        {
            get
            {
                dynamic payloadString = RpcContext.GetContext().GetAttachment(PayloadKey);
                if (!string.IsNullOrEmpty(payloadString))
                {
                    dynamic payload = JsonConvert.DeserializeObject(payloadString);
                    return payload.userId ?? payload.UserId;
                }
                return null;
            }
        }

        public override string UserName
        {
            get
            {
                var payloadString = (string)RpcContext.GetContext().GetAttachment(PayloadKey);
                if (!string.IsNullOrEmpty(payloadString))
                {
                    dynamic payload = JsonConvert.DeserializeObject(payloadString);
                    return payload.userName ?? payload.UserName;
                }

                return null;
            }
        }
    }
}