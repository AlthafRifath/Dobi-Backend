using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Payments;
using Dobi.Domain.Payments;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.RecordPayment
{
    public sealed class RecordPaymentCommandHandler
    : IRequestHandler<RecordPaymentCommand, PaymentResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RecordPaymentCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PaymentResponse> Handle(
            RecordPaymentCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var order = await _dbContext.Orders
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            var orderStatusCode = LookupValueHelper.GetCode(order.CurrentStatus);

            if (orderStatusCode is not OrderStatusCodes.CollectedDelivered
                and not OrderStatusCodes.Closed)
            {
                throw new ConflictException("Payments can only be recorded after the order has been collected or delivered.");
            }

            var paymentMethod = await _dbContext.PaymentMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId, cancellationToken);

            if (paymentMethod is null)
            {
                throw new NotFoundException("Payment method", request.PaymentMethodId);
            }

            var methodCode = LookupValueHelper.GetCode(paymentMethod);

            var currentSummary = await PaymentStatusUpdater.GetSummaryAsync(
                _dbContext,
                order,
                cancellationToken);

            if (currentSummary.OutstandingAmount <= 0)
            {
                throw new ConflictException("This order is already fully paid or fully covered by pending payments.");
            }

            if (request.Amount > currentSummary.OutstandingAmount)
            {
                throw new BadRequestException($"Payment amount cannot exceed outstanding amount {currentSummary.OutstandingAmount:N2}.");
            }

            if (methodCode == PaymentMethodCodes.Cheque)
            {
                if (string.IsNullOrWhiteSpace(request.ChequeNo))
                {
                    throw new BadRequestException("Cheque number is required for cheque payments.");
                }

                if (string.IsNullOrWhiteSpace(request.ChequeBankName))
                {
                    throw new BadRequestException("Cheque bank name is required for cheque payments.");
                }

                if (!request.ChequeDate.HasValue)
                {
                    throw new BadRequestException("Cheque date is required for cheque payments.");
                }
            }

            var paymentStatuses = await _dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var paymentStatus = methodCode == PaymentMethodCodes.Cheque
                ? paymentStatuses.First(x => LookupValueHelper.GetCode(x) == PaymentStatusCodes.PendingClearance)
                : paymentStatuses.First(x => LookupValueHelper.GetCode(x) == PaymentStatusCodes.Paid);

            var now = _dateTimeProvider.UtcNow;

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = request.Amount,
                PaymentMethodId = paymentMethod.Id,
                PaymentStatusId = paymentStatus.Id,
                PaidAt = methodCode == PaymentMethodCodes.Cheque ? null : now,
                ReceivedByUserId = _currentUserService.UserId.Value,
                ReferenceNo = string.IsNullOrWhiteSpace(request.ReferenceNo)
                    ? null
                    : request.ReferenceNo.Trim(),

                ChequeNo = methodCode == PaymentMethodCodes.Cheque
                    ? request.ChequeNo?.Trim()
                    : null,
                ChequeBankName = methodCode == PaymentMethodCodes.Cheque
                    ? request.ChequeBankName?.Trim()
                    : null,
                ChequeDate = methodCode == PaymentMethodCodes.Cheque
                    ? request.ChequeDate
                    : null,

                CreatedAt = now,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Payments.Add(payment);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedOrder = await _dbContext.Orders
                .FirstAsync(x => x.Id == order.Id, cancellationToken);

            var summary = await PaymentStatusUpdater.RecalculateOrderPaymentStatusAsync(
                _dbContext,
                _dateTimeProvider,
                _currentUserService.UserId,
                savedOrder,
                cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedPayment = await LoadPaymentAsync(payment.Id, cancellationToken);

            return PaymentResponseMapper.Map(savedPayment, summary);
        }

        private async Task<Payment> LoadPaymentAsync(
            int paymentId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Payments
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.PaymentMethod)
                .Include(x => x.PaymentStatus)
                .FirstAsync(x => x.Id == paymentId, cancellationToken);
        }
    }
}
