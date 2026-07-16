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

namespace Dobi.Application.Features.Reports.GetPendingChequeReport
{
    public sealed class GetPendingChequeReportQueryHandler
    : IRequestHandler<GetPendingChequeReportQuery, PagedResponse<PendingChequeReportResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPendingChequeReportQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<PendingChequeReportResponse>> Handle(
            GetPendingChequeReportQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Payments
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.Customer)
                .Include(x => x.PaymentMethod)
                .Include(x => x.PaymentStatus)
                .Where(x =>
                    x.PaymentMethod.MethodCode == PaymentMethodCodes.Cheque &&
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.PendingClearance)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                    x.Order.Customer.FullName.ToLower().Contains(searchTerm) ||
                    x.Order.Customer.MobileNo.ToLower().Contains(searchTerm) ||
                    (x.ChequeNo != null && x.ChequeNo.ToLower().Contains(searchTerm)) ||
                    (x.ChequeBankName != null && x.ChequeBankName.ToLower().Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var payments = await query
                .OrderBy(x => x.ChequeDate)
                .ThenBy(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = payments
                .Select(payment => new PendingChequeReportResponse(
                    payment.Id,
                    payment.OrderId,
                    payment.Order.OrderNo,
                    payment.Order.CustomerId,
                    payment.Order.Customer.FullName,
                    payment.Order.Customer.MobileNo,
                    payment.Amount,
                    payment.ChequeNo,
                    payment.ChequeBankName,
                    payment.ChequeDate,
                    payment.CreatedAt,
                    LookupValueHelper.GetName(payment.PaymentStatus)))
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<PendingChequeReportResponse>(
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
