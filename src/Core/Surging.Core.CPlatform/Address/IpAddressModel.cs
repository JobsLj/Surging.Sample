using Newtonsoft.Json;
using System.Net;

namespace Surging.Core.CPlatform.Address
{
    public sealed class IpAddressModel : AddressModel
    {
        #region Constructor

 
        public IpAddressModel()
        {
        }


        public IpAddressModel(string ip, int port)
        {
            Ip = ip;
            Port = port; 
        }

        #endregion Constructor

        #region Property

        public string Ip { get; set; }

        public int Port { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WanIp { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? WsPort { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MqttPort { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? HttpPort { get; set; }

        #endregion Property

        #region Overrides of AddressModel


        public override EndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Ip), Port);
        }

        public override string ToString()
        {
            return string.Concat(new string[] { Ip, ":", Port.ToString() });
        }

        #endregion Overrides of AddressModel
    }
}