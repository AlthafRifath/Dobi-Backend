using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationsByOrder
{
    public sealed class GetNotificationsByOrderQueryValidator
    : AbstractValidator<GetNotificationsByOrderQuery>
    {
        public GetNotificationsByOrderQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
