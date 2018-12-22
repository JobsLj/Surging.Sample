using Newtonsoft.Json;
using System.Net;

namespace Surging.Core.CPlatform.Address
{
    public sealed class MappingAddressModel : AddressModel
    {
        public MappingAddressModel()
        {
        }

        public MappingAddressModel(string address, int port, string mappingIp, int mappingPort)
        {
            InnerIp = AddressHelper.GetIpFromAddress(address);
            InnerPort = port;
            MappingIp = mappingIp;
            MappingPort = mappingPort;
        }

        public string InnerIp { get; set; }

        public int InnerPort { get; set; }

        public string MappingIp { get; set; }

        public int MappingPort { get; set; }

        [JsonIgnore]
        public EndPointMode EndPointMode
        {
            get
            {
                if (string.IsNullOrEmpty(AppConfig.ServerOptions.MappingIP))
                {
                    return EndPointMode.InnerEndPoint;
                }
                return EndPointMode.MappingEndPoint;
            }
        }

        public override EndPoint CreateEndPoint()
        {
            if (EndPointMode == EndPointMode.MappingEndPoint)
            {
                return new IPEndPoint(IPAddress.Parse(InnerIp), InnerPort);
            }
            return new IPEndPoint(IPAddress.Parse(MappingIp), MappingPort);
        }

        public override string ToString()
        {
            if (EndPointMode == EndPointMode.MappingEndPoint)
            {
                return string.Concat(new string[] { InnerIp, ":", InnerPort.ToString() });
            }
            return string.Concat(new string[] { MappingIp, ":", MappingPort.ToString() });
        }
    }
}