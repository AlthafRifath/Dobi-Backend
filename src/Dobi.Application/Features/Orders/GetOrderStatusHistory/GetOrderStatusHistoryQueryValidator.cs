using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrderStatusHistory
{
    public sealed class GetOrderStatusHistoryQueryValidator : AbstractValidator<GetOrderStatusHistoryQuery>
    {
        public GetOrderStatusHistoryQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
