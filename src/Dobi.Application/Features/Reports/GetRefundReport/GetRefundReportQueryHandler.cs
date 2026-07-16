using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetRefundReport
{
    public sealed class GetRefundReportQueryHandler
    : IRequestHandler<GetRefundReportQuery, PagedResponse<RefundReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetRefundReportQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<RefundReportResponse>> Handle(
            GetRefundReportQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Refunds
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.RefundStatus)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                    x.Reason.ToLower().Contains(searchTerm));
            }

            if (request.RefundStatusId.HasValue)
            {
                query = query.Where(x => x.RefundStatusId == request.RefundStatusId.Value);
            }

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(x => x.CreatedAt >= fromDate);
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(x => x.CreatedAt <= toDate);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var refunds = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = refunds
                .Select(refund => new RefundReportResponse(
                    refund.Id,
                    refund.OrderId,
                    refund.Order.OrderNo,
                    refund.PaymentId,
                    refund.Amount,
                    refund.Reason,
                    refund.RefundStatusId,
                    LookupValueHelper.GetName(refund.RefundStatus),
                    refund.CreatedAt,
                    refund.RefundedAt))
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<RefundReportResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
