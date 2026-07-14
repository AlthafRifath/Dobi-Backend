using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
