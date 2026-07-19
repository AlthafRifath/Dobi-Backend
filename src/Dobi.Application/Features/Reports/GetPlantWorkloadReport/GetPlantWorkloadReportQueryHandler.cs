using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Reports.GetPlantWorkloadReport
{
    public sealed class GetPlantWorkloadReportQueryHandler
        : IRequestHandler<
            GetPlantWorkloadReportQuery,
            IReadOnlyCollection<PlantWorkloadReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantWorkloadReportQueryHandler(
            IDobiDbContext dbContext)
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
                var fromDateUtc = request.FromDate.Value.ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc);

                query = query.Where(x =>
                    x.ReceivedAtPlant >= fromDateUtc);
            }

            if (request.ToDate.HasValue)
            {
                var toDateExclusiveUtc = request.ToDate.Value
                    .AddDays(1)
                    .ToDateTime(
                        TimeOnly.MinValue,
                        DateTimeKind.Utc);

                query = query.Where(x =>
                    x.ReceivedAtPlant < toDateExclusiveUtc);
            }

            var records = await query
                .ToArrayAsync(cancellationToken);

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
                    group.Count(x =>
                        x.Order.CurrentStatus.StatusCode ==
                        OrderStatusCodes.Processing),
                    group.Count(x =>
                        x.Order.CurrentStatus.StatusCode ==
                        OrderStatusCodes.QcPending),
                    group.Count(x =>
                        x.Order.CurrentStatus.StatusCode ==
                        OrderStatusCodes.QcFailed),
                    group.Count(x =>
                        x.Order.CurrentStatus.StatusCode ==
                        OrderStatusCodes.Packed),
                    group.Count(x =>
                        x.Order.CurrentStatus.StatusCode ==
                        OrderStatusCodes.ReadyForOutletReturn)))
                .OrderByDescending(x => x.TotalOrders)
                .ToArray();
        }
    }
}