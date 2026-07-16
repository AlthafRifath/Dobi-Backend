using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionById
{
    public sealed class GetCollectionByIdQueryValidator : AbstractValidator<GetCollectionByIdQuery>
    {
        public GetCollectionByIdQueryValidator()
        {
            RuleFor(x => x.OrderCollectionId)
                .GreaterThan(0);
        }
    }
}
