using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public class DataAccessException : CPlatformException
    {
        public DataAccessException(string message, Exception innerException = null) : base(message, innerException, StatusCode.BusinessError)
        {
        }

        public DataAccessException(string message) : base(message, StatusCode.BusinessError)
        {
        }
    }
}