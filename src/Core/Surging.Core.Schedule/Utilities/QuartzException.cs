using System;

namespace Surging.Core.Schedule.Utilities
{
    internal class QuartzException : Exception
    {
        public QuartzException()
        {
        }

        public QuartzException(string message)
            : base(message)
        {
            Message = message;
        }

        public QuartzException(string message, Exception e)
            : base(message, e)
        {
            Message = message;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        private new string Message
        {
            get;
            set;
        }
    }
}