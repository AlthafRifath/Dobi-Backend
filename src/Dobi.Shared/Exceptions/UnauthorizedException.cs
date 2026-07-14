using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Exceptions
{
    public sealed class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
