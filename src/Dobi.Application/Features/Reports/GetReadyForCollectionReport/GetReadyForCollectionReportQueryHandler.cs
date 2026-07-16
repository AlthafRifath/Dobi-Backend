using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetReadyForCollectionReport
{
    public sealed class GetReadyForCollectionReportQueryHandler
    : IRequestHandler<GetReadyForCollectionReportQuery, PagedResponse<ReadyForCollectionReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetReadyForCollectionReportQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<ReadyForCollectionReportResponse>> Handle(
            GetReadyForCollectionReportQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.PaymentStatus)
                .Include(x => x.CurrentStatus)
                .Where(x => x.CurrentStatus.StatusCode == OrderStatusCodes.ReadyForCollection)
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
                .OrderBy(x => x.ExpectedReturnDate)
                .ThenBy(x => x.OrderNo)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = orders
                .Select(order => new ReadyForCollectionReportResponse(
                    order.Id,
                    order.OrderNo,
                    order.OrderDate,
                    order.CustomerId,
                    order.Customer.FullName,
                    order.Customer.MobileNo,
                    LookupValueHelper.GetCode(order.Customer.CustomerType),
                    LookupValueHelper.GetName(order.Customer.CustomerType),
                    order.BranchId,
                    order.Branch.BranchName,
                    order.TotalAmount,
                    order.PaymentStatusId,
                    LookupValueHelper.GetName(order.PaymentStatus)))
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<ReadyForCollectionReportResponse>(
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
