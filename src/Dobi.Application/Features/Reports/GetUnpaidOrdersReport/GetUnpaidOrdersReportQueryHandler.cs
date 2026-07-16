using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Application.Features.Payments;
using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetUnpaidOrdersReport
{
    public sealed class GetUnpaidOrdersReportQueryHandler
    : IRequestHandler<GetUnpaidOrdersReportQuery, PagedResponse<UnpaidOrderReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetUnpaidOrdersReportQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<UnpaidOrderReportResponse>> Handle(
            GetUnpaidOrdersReportQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Where(x =>
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.Unpaid ||
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.PendingClearance ||
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.PartiallyPaid ||
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.PartiallyPaidPendingClearance)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.OrderNo.ToLower().Contains(searchTerm) ||
                    x.Customer.FullName.ToLower().Contains(searchTerm) ||
                    x.Customer.MobileNo.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var orders = await query
                .OrderByDescending(x => x.OrderDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = new List<UnpaidOrderReportResponse>();

            foreach (var order in orders)
            {
                var summary = await PaymentStatusUpdater.GetSummaryAsync(
                    _dbContext,
                    order,
                    cancellationToken);

                items.Add(new UnpaidOrderReportResponse(
                    order.Id,
                    order.OrderNo,
                    order.OrderDate,
                    order.CustomerId,
                    order.Customer.FullName,
                    order.Customer.MobileNo,
                    order.BranchId,
                    order.Branch.BranchName,
                    summary.OrderTotalAmount,
                    summary.PaidAmount,
                    summary.PendingClearanceAmount,
                    summary.OutstandingAmount,
                    order.PaymentStatusId,
                    LookupValueHelper.GetName(order.PaymentStatus),
                    order.CurrentStatusId,
                    LookupValueHelper.GetName(order.CurrentStatus)));
            }

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<UnpaidOrderReportResponse>(
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
