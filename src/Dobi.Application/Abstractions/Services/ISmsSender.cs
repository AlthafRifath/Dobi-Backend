using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public interface ISmsSender
    {
        Task<SmsSendResult> SendAsync(
            string recipientMobileNo,
            string message,
            CancellationToken cancellationToken = default);
    }
}
