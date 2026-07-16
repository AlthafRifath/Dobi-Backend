using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.RetryNotification
{
    public sealed class RetryNotificationCommandValidator
    : AbstractValidator<RetryNotificationCommand>
    {
        public RetryNotificationCommandValidator()
        {
            RuleFor(x => x.NotificationId)
                .GreaterThan(0);
        }
    }
}
