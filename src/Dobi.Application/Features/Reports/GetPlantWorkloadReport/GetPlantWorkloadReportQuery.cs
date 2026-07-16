using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetPlantWorkloadReport
{
    public sealed record GetPlantWorkloadReportQuery(
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<IReadOnlyCollection<PlantWorkloadReportResponse>>;
}
