using Dobi.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Services
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
