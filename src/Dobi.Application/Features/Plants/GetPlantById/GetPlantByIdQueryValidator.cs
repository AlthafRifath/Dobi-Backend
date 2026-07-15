using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.GetPlantById
{
    public sealed class GetPlantByIdQueryValidator : AbstractValidator<GetPlantByIdQuery>
    {
        public GetPlantByIdQueryValidator()
        {
            RuleFor(x => x.PlantId)
                .GreaterThan(0);
        }
    }
}
