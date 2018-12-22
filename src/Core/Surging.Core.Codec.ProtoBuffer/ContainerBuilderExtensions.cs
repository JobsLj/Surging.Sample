using Surging.Core.CPlatform;

namespace Surging.Core.Codec.ProtoBuffer
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 基于protobuf序列化
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceBuilder UseProtoBufferCodec(this IServiceBuilder builder)
        {
            return builder.UseCodec<ProtoBufferTransportMessageCodecFactory>();
        }
    }
}