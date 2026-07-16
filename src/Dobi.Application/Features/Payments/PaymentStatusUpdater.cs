using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments
{
    internal static class PaymentStatusUpdater
    {
        public static async Task<PaymentAmountSummary> RecalculateOrderPaymentStatusAsync(
            IDobiDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            int? currentUserId,
            Order order,
            CancellationToken cancellationToken)
        {
            var paymentStatuses = await dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var paidStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Paid);
            var pendingClearanceStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.PendingClearance);
            var unpaidStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Unpaid);
            var partiallyPaidStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.PartiallyPaid);
            var partiallyPaidPendingStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.PartiallyPaidPendingClearance);

            var paidAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == paidStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var pendingAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == pendingClearanceStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var failedStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Failed);

            var failedAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == failedStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var orderTotal = order.TotalAmount;

            PaymentStatus newOrderPaymentStatus;

            if (paidAmount >= orderTotal)
            {
                newOrderPaymentStatus = paidStatus;
            }
            else if (paidAmount > 0 && pendingAmount > 0)
            {
                newOrderPaymentStatus = partiallyPaidPendingStatus;
            }
            else if (paidAmount > 0)
            {
                newOrderPaymentStatus = partiallyPaidStatus;
            }
            else if (pendingAmount > 0)
            {
                newOrderPaymentStatus = pendingClearanceStatus;
            }
            else
            {
                newOrderPaymentStatus = unpaidStatus;
            }

            order.PaymentStatusId = newOrderPaymentStatus.Id;
            order.UpdatedAt = dateTimeProvider.UtcNow;
            order.UpdatedByUserId = currentUserId;

            await CloseOrderIfFullyPaidAsync(
                dbContext,
                dateTimeProvider,
                currentUserId,
                order,
                LookupValueHelper.GetCode(newOrderPaymentStatus),
                cancellationToken);

            var outstandingAmount = Math.Max(orderTotal - paidAmount - pendingAmount, 0);

            return new PaymentAmountSummary(
                orderTotal,
                paidAmount,
                pendingAmount,
                failedAmount,
                outstandingAmount,
                newOrderPaymentStatus.Id,
                LookupValueHelper.GetCode(newOrderPaymentStatus),
                LookupValueHelper.GetName(newOrderPaymentStatus));
        }

        public static async Task<PaymentAmountSummary> GetSummaryAsync(
            IDobiDbContext dbContext,
            Order order,
            CancellationToken cancellationToken)
        {
            var paymentStatuses = await dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var paidStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Paid);
            var pendingClearanceStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.PendingClearance);
            var failedStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Failed);

            var paidAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == paidStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var pendingAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == pendingClearanceStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var failedAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == failedStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var orderPaymentStatus = await dbContext.PaymentStatuses
                .AsNoTracking()
                .FirstAsync(x => x.Id == order.PaymentStatusId, cancellationToken);

            var outstandingAmount = Math.Max(order.TotalAmount - paidAmount - pendingAmount, 0);

            return new PaymentAmountSummary(
                order.TotalAmount,
                paidAmount,
                pendingAmount,
                failedAmount,
                outstandingAmount,
                orderPaymentStatus.Id,
                LookupValueHelper.GetCode(orderPaymentStatus),
                LookupValueHelper.GetName(orderPaymentStatus));
        }

        public static async Task MarkOrderFullyRefundedIfApplicableAsync(
            IDobiDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            int? currentUserId,
            Order order,
            CancellationToken cancellationToken)
        {
            var paymentStatuses = await dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var paidStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Paid);
            var refundedOrderStatus = GetPaymentStatus(paymentStatuses, PaymentStatusCodes.Refunded);

            var totalPaidAmount = await dbContext.Payments
                .Where(x => x.OrderId == order.Id && x.PaymentStatusId == paidStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var refundStatuses = await dbContext.RefundStatuses
                .ToListAsync(cancellationToken);

            var paidRefundStatus = refundStatuses.First(x =>
                LookupValueHelper.GetCode(x) == RefundStatusCodes.Paid);

            var totalRefundedAmount = await dbContext.Refunds
                .Where(x => x.OrderId == order.Id && x.RefundStatusId == paidRefundStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            if (totalPaidAmount > 0 && totalRefundedAmount >= totalPaidAmount)
            {
                order.PaymentStatusId = refundedOrderStatus.Id;
                order.UpdatedAt = dateTimeProvider.UtcNow;
                order.UpdatedByUserId = currentUserId;
            }
        }

        private static PaymentStatus GetPaymentStatus(
            IReadOnlyCollection<PaymentStatus> statuses,
            string statusCode)
        {
            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Payment status '{statusCode}' was not found.");
            }

            return status;
        }

        private static async Task CloseOrderIfFullyPaidAsync(
            IDobiDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            int? currentUserId,
            Order order,
            string orderPaymentStatusCode,
            CancellationToken cancellationToken)
        {
            if (orderPaymentStatusCode != PaymentStatusCodes.Paid)
            {
                return;
            }

            var orderStatuses = await dbContext.OrderStatuses
                .ToListAsync(cancellationToken);

            var currentOrderStatus = orderStatuses.First(x => x.Id == order.CurrentStatusId);
            var currentOrderStatusCode = LookupValueHelper.GetCode(currentOrderStatus);

            if (currentOrderStatusCode == OrderStatusCodes.Closed)
            {
                return;
            }

            if (currentOrderStatusCode != OrderStatusCodes.CollectedDelivered)
            {
                return;
            }

            var closedStatus = orderStatuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == OrderStatusCodes.Closed);

            if (closedStatus is null)
            {
                throw new InvalidOperationException("Order status 'CLOSED' was not found.");
            }

            order.CurrentStatusId = closedStatus.Id;
            order.UpdatedAt = dateTimeProvider.UtcNow;
            order.UpdatedByUserId = currentUserId;

            dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = closedStatus.Id,
                Remarks = "Order fully paid and closed.",
                ChangedByUserId = currentUserId!.Value,
                ChangedAt = dateTimeProvider.UtcNow
            });
        }
    }
}
