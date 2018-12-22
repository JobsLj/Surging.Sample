using Surging.Core.CPlatform;

namespace Surging.Core.Codec.MessagePack
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 基于MessagePack序列化
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceBuilder UseMessagePackCodec(this IServiceBuilder builder)
        {
            return builder.UseCodec<MessagePackTransportMessageCodecFactory>();
        }
    }
}