namespace Surging.Core.CPlatform.Configurations
{
    /// <summary>
    /// 通信协议端口
    /// </summary>
    public class ProtocolPortOptions
    {
        public int MQTTPort { get; set; } = 97;

        public int HttpPort { get; set; } = 80;

        public int WSPort { get; set; } = 96;
    }
}