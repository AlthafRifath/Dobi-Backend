using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Refunds;
using Dobi.Domain.Payments;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.CreateRefund
{
    public sealed class CreateRefundCommandHandler
    : IRequestHandler<CreateRefundCommand, RefundResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateRefundCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<RefundResponse> Handle(
            CreateRefundCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var order = await _dbContext.Orders
                .Include(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            Payment? payment = null;

            if (request.PaymentId.HasValue)
            {
                payment = await _dbContext.Payments
                    .Include(x => x.PaymentStatus)
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentId.Value, cancellationToken);

                if (payment is null)
                {
                    throw new NotFoundException("Payment", request.PaymentId.Value);
                }

                if (payment.OrderId != order.Id)
                {
                    throw new BadRequestException("Selected payment does not belong to this order.");
                }

                var paymentStatusCode = LookupValueHelper.GetCode(payment.PaymentStatus);

                if (paymentStatusCode != PaymentStatusCodes.Paid)
                {
                    throw new ConflictException("Only cleared/paid payments can be refunded.");
                }
            }

            var refundableAmount = request.PaymentId.HasValue
                ? await GetRefundableAmountForPaymentAsync(
                    request.PaymentId.Value,
                    payment!.Amount,
                    cancellationToken)
                : await GetRefundableAmountForOrderAsync(
                    order.Id,
                    cancellationToken);

            if (request.Amount > refundableAmount)
            {
                throw new BadRequestException(
                    $"Refund amount cannot exceed refundable amount {refundableAmount:N2}.");
            }

            var pendingStatus = await GetRefundStatusByCodeAsync(
                RefundStatusCodes.Pending,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            var refund = new Refund
            {
                OrderId = order.Id,
                PaymentId = request.PaymentId,
                Amount = request.Amount,
                Reason = request.Reason.Trim(),
                RefundStatusId = pendingStatus.Id,
                CreatedAt = now,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Refunds.Add(refund);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedRefund = await LoadRefundAsync(refund.Id, cancellationToken);

            return RefundResponseMapper.Map(savedRefund);
        }

        private async Task<decimal> GetRefundableAmountForPaymentAsync(
            int paymentId,
            decimal paymentAmount,
            CancellationToken cancellationToken)
        {
            var activeRefundStatusIds = await GetActiveRefundStatusIdsAsync(cancellationToken);

            var alreadyReservedOrRefunded = await _dbContext.Refunds
                .Where(x => x.PaymentId == paymentId &&
                            activeRefundStatusIds.Contains(x.RefundStatusId))
                .SumAsync(x => x.Amount, cancellationToken);

            return Math.Max(paymentAmount - alreadyReservedOrRefunded, 0);
        }

        private async Task<decimal> GetRefundableAmountForOrderAsync(
            int orderId,
            CancellationToken cancellationToken)
        {
            var paidPaymentStatus = await GetPaymentStatusByCodeAsync(
                PaymentStatusCodes.Paid,
                cancellationToken);

            var totalPaid = await _dbContext.Payments
                .Where(x => x.OrderId == orderId &&
                            x.PaymentStatusId == paidPaymentStatus.Id)
                .SumAsync(x => x.Amount, cancellationToken);

            var activeRefundStatusIds = await GetActiveRefundStatusIdsAsync(cancellationToken);

            var alreadyReservedOrRefunded = await _dbContext.Refunds
                .Where(x => x.OrderId == orderId &&
                            activeRefundStatusIds.Contains(x.RefundStatusId))
                .SumAsync(x => x.Amount, cancellationToken);

            return Math.Max(totalPaid - alreadyReservedOrRefunded, 0);
        }

        private async Task<int[]> GetActiveRefundStatusIdsAsync(
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.RefundStatuses
                .ToListAsync(cancellationToken);

            return statuses
                .Where(x =>
                {
                    var code = LookupValueHelper.GetCode(x);

                    return code is RefundStatusCodes.Pending
                        or RefundStatusCodes.Approved
                        or RefundStatusCodes.Paid;
                })
                .Select(x => x.Id)
                .ToArray();
        }

        private async Task<Dobi.Domain.Orders.PaymentStatus> GetPaymentStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Payment status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<RefundStatus> GetRefundStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.RefundStatuses
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Refund status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<Refund> LoadRefundAsync(
            int refundId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Refunds
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.RefundStatus)
                .FirstAsync(x => x.Id == refundId, cancellationToken);
        }
    }
}
