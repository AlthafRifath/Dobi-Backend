using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionByOrder
{
    public sealed class GetCollectionByOrderQueryValidator : AbstractValidator<GetCollectionByOrderQuery>
    {
        public GetCollectionByOrderQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
