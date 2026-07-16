using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Payments;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.MarkChequeCleared
{
    public sealed class MarkChequeClearedCommandHandler
    : IRequestHandler<MarkChequeClearedCommand, PaymentResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MarkChequeClearedCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PaymentResponse> Handle(
            MarkChequeClearedCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var payment = await _dbContext.Payments
                .Include(x => x.PaymentMethod)
                .Include(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.PaymentId, cancellationToken);

            if (payment is null)
            {
                throw new NotFoundException("Payment", request.PaymentId);
            }

            var methodCode = LookupValueHelper.GetCode(payment.PaymentMethod);

            if (methodCode != PaymentMethodCodes.Cheque)
            {
                throw new ConflictException("Only cheque payments can be marked as cleared.");
            }

            var currentPaymentStatusCode = LookupValueHelper.GetCode(payment.PaymentStatus);

            if (currentPaymentStatusCode != PaymentStatusCodes.PendingClearance)
            {
                throw new ConflictException("Only pending clearance cheque payments can be marked as cleared.");
            }

            var paidStatus = await _dbContext.PaymentStatuses
                .ToListAsync(cancellationToken);

            var paidPaymentStatus = paidStatus.First(x =>
                LookupValueHelper.GetCode(x) == PaymentStatusCodes.Paid);

            var now = _dateTimeProvider.UtcNow;

            payment.PaymentStatusId = paidPaymentStatus.Id;
            payment.PaidAt = now;
            payment.ChequeClearedAt = now;
            payment.UpdatedAt = now;
            payment.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var order = await _dbContext.Orders
                .FirstAsync(x => x.Id == payment.OrderId, cancellationToken);

            var summary = await PaymentStatusUpdater.RecalculateOrderPaymentStatusAsync(
                _dbContext,
                _dateTimeProvider,
                _currentUserService.UserId,
                order,
                cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedPayment = await LoadPaymentAsync(payment.Id, cancellationToken);

            return PaymentResponseMapper.Map(savedPayment, summary);
        }

        private async Task<Dobi.Domain.Payments.Payment> LoadPaymentAsync(
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
