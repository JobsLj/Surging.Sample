using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    /// <summary>
    /// 基础异常类。
    /// </summary>
    public class CPlatformException : Exception
    {
        public StatusCode ExceptionCode { get; }

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        public CPlatformException(string message, Exception innerException = null, StatusCode exceptionCode = StatusCode.CPlatformError) : base(message, innerException)
        {
            ExceptionCode = exceptionCode;
        }

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        public CPlatformException(string message, StatusCode exceptionCode = StatusCode.CPlatformError) : base(message)
        {
            ExceptionCode = exceptionCode;
        }

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        public CPlatformException(string message) : base(message)
        {
            ExceptionCode = StatusCode.CPlatformError;
        }
    }
}