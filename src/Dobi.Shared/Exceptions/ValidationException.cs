using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Exceptions
{
    public sealed class ValidationException : AppException
    {
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        public IReadOnlyDictionary<string, string[]> Errors { get; }
    }
}
