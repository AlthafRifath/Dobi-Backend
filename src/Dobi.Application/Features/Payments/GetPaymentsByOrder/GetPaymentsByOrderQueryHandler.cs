using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Payments;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPaymentsByOrder
{
    public sealed class GetPaymentsByOrderQueryHandler
    : IRequestHandler<GetPaymentsByOrderQuery, OrderPaymentsResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPaymentsByOrderQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderPaymentsResponse> Handle(
            GetPaymentsByOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            var payments = await _dbContext.Payments
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.PaymentMethod)
                .Include(x => x.PaymentStatus)
                .Where(x => x.OrderId == request.OrderId)
                .OrderByDescending(x => x.CreatedAt)
                .ToArrayAsync(cancellationToken);

            var summary = await PaymentStatusUpdater.GetSummaryAsync(
                _dbContext,
                order,
                cancellationToken);

            var responsePayments = payments
                .Select(x => PaymentResponseMapper.Map(x, summary))
                .ToArray();

            return new OrderPaymentsResponse(
                order.Id,
                order.OrderNo,
                summary.OrderTotalAmount,
                summary.PaidAmount,
                summary.PendingClearanceAmount,
                summary.FailedAmount,
                summary.OutstandingAmount,
                summary.OrderPaymentStatusId,
                summary.OrderPaymentStatusCode,
                summary.OrderPaymentStatusName,
                responsePayments);
        }
    }
}
