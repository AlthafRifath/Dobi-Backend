using Dobi.Contracts.Common;
using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessing
{
    public sealed record GetPlantProcessingQuery(
        int PageNumber,
        int PageSize,
        int? PlantId,
        int? OrderId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<PlantProcessingResponse>>;
}
