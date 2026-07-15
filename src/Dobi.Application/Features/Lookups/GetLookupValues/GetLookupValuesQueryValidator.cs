using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Lookups.GetLookupValues
{
    public sealed class GetLookupValuesQueryValidator : AbstractValidator<GetLookupValuesQuery>
    {
        public GetLookupValuesQueryValidator()
        {
            RuleFor(x => x.LookupType)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
