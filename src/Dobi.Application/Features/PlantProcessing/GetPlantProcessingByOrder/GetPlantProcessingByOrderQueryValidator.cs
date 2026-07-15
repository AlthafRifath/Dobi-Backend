using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingByOrder
{
    public sealed class GetPlantProcessingByOrderQueryValidator : AbstractValidator<GetPlantProcessingByOrderQuery>
    {
        public GetPlantProcessingByOrderQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
