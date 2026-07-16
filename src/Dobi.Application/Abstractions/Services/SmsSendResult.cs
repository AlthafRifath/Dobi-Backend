using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public sealed record SmsSendResult(
        bool IsSuccess,
        string? ErrorMessage);
}
