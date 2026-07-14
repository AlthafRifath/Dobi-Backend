using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Exceptions
{
    public sealed class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message)
        {
        }
    }
}
