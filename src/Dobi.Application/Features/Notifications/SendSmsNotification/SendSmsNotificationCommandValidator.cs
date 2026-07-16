using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.SendSmsNotification
{
    public sealed class SendSmsNotificationCommandValidator
    : AbstractValidator<SendSmsNotificationCommand>
    {
        public SendSmsNotificationCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .When(x => x.OrderId.HasValue);

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .When(x => x.CustomerId.HasValue);

            RuleFor(x => x.RecipientMobileNo)
                .MaximumLength(20);

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x)
                .Must(x => x.OrderId.HasValue || x.CustomerId.HasValue)
                .WithMessage("Either orderId or customerId is required.");
        }
    }
}
