using Dobi.Contracts.Common;
using Dobi.Contracts.Plants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.GetPlants
{
    public sealed record GetPlantsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        bool? IsActive) : IRequest<PagedResponse<PlantResponse>>;
}
