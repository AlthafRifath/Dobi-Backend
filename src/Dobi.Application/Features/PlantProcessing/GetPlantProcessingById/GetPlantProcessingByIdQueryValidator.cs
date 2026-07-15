using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingById
{
    public sealed class GetPlantProcessingByIdQueryValidator : AbstractValidator<GetPlantProcessingByIdQuery>
    {
        public GetPlantProcessingByIdQueryValidator()
        {
            RuleFor(x => x.PlantProcessingId)
                .GreaterThan(0);
        }
    }
}
