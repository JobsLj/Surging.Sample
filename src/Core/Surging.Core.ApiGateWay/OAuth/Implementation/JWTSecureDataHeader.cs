namespace Surging.Core.ApiGateWay.OAuth
{
    public class JWTSecureDataHeader
    {
        public JWTSecureDataType Type { get; set; }

        /// <summary>
        /// 加解密模式
        /// </summary>
        public EncryptMode EncryptMode { get; set; }

        /// <summary>
        /// 时间标记/时间标签
        /// </summary>
        public string TimeStamp { get; set; }
    }
}