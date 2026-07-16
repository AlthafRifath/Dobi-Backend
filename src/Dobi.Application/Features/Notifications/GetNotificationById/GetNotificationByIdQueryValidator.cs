using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationById
{
    public sealed class GetNotificationByIdQueryValidator
    : AbstractValidator<GetNotificationByIdQuery>
    {
        public GetNotificationByIdQueryValidator()
        {
            RuleFor(x => x.NotificationId)
                .GreaterThan(0);
        }
    }
}
