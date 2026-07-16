using Dobi.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Notifications
{
    public sealed class DevelopmentSmsSender : ISmsSender
    {
        private readonly ILogger<DevelopmentSmsSender> _logger;

        public DevelopmentSmsSender(ILogger<DevelopmentSmsSender> logger)
        {
            _logger = logger;
        }

        public Task<SmsSendResult> SendAsync(
            string recipientMobileNo,
            string message,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(recipientMobileNo))
            {
                return Task.FromResult(new SmsSendResult(
                    false,
                    "Recipient mobile number is required."));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return Task.FromResult(new SmsSendResult(
                    false,
                    "SMS message is required."));
            }

            _logger.LogInformation(
                "Development SMS sent to {Recipient}. Message: {Message}",
                recipientMobileNo,
                message);

            return Task.FromResult(new SmsSendResult(
                true,
                null));
        }
    }
}
