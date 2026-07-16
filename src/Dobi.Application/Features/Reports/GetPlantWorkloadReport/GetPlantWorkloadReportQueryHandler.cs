using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetPlantWorkloadReport
{
    public sealed class GetPlantWorkloadReportQueryHandler
    : IRequestHandler<GetPlantWorkloadReportQuery, IReadOnlyCollection<PlantWorkloadReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantWorkloadReportQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<PlantWorkloadReportResponse>> Handle(
            GetPlantWorkloadReportQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.PlantProcessings
                .AsNoTracking()
                .Include(x => x.Plant)
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .AsQueryable();

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(x => x.ReceivedAtPlant >= fromDate);
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(x => x.ReceivedAtPlant <= toDate);
            }

            var records = await query.ToArrayAsync(cancellationToken);

            return records
                .GroupBy(x => new
                {
                    x.PlantId,
                    x.Plant.PlantName
                })
                .Select(group => new PlantWorkloadReportResponse(
                    group.Key.PlantId,
                    group.Key.PlantName,
                    group.Count(),
                    group.Count(x => x.Order.CurrentStatus.StatusCode == OrderStatusCodes.Processing),
                    group.Count(x => x.Order.CurrentStatus.StatusCode == OrderStatusCodes.QcPending),
                    group.Count(x => x.Order.CurrentStatus.StatusCode == OrderStatusCodes.QcFailed),
                    group.Count(x => x.Order.CurrentStatus.StatusCode == OrderStatusCodes.Packed),
                    group.Count(x => x.Order.CurrentStatus.StatusCode == OrderStatusCodes.ReadyForOutletReturn)))
                .OrderByDescending(x => x.TotalOrders)
                .ToArray();
        }
    }
}
