using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    /// <summary>
    /// 远程执行异常（由服务端转发至客户端的异常信息）。
    /// </summary>
    public class CPlatformCommunicationException : CPlatformException
    {
        public CPlatformCommunicationException(string message, Exception innerException = null) : base(message, innerException, StatusCode.BusinessError)
        {
        }

        public CPlatformCommunicationException(string message) : base(message, StatusCode.BusinessError)
        {
        }
    }
}