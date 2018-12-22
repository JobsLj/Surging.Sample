using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    /// <summary>
    /// 通讯异常（与服务端进行通讯时发生的异常）。
    /// </summary>
    public class CommunicationException : CPlatformException
    {
        public CommunicationException(string message, Exception innerException = null) : base(message, innerException, StatusCode.BusinessError)
        {
        }

        public CommunicationException(string message) : base(message, StatusCode.BusinessError)
        {
        }
    }
}