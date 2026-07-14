using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Exceptions
{
    public sealed class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string entityName, object key)
            : base($"{entityName} with key '{key}' was not found.")
        {
        }
    }
}
