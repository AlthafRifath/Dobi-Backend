using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessing
{
    public sealed class GetPlantProcessingQueryValidator : AbstractValidator<GetPlantProcessingQuery>
    {
        public GetPlantProcessingQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.PlantId)
                .GreaterThan(0)
                .When(x => x.PlantId.HasValue);

            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .When(x => x.OrderId.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
