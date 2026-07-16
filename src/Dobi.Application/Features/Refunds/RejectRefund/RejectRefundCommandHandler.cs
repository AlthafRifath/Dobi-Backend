using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Refunds;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.RejectRefund
{
    public sealed class RejectRefundCommandHandler
    : IRequestHandler<RejectRefundCommand, RefundResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RejectRefundCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<RefundResponse> Handle(
            RejectRefundCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var refund = await _dbContext.Refunds
                .Include(x => x.RefundStatus)
                .FirstOrDefaultAsync(x => x.Id == request.RefundId, cancellationToken);

            if (refund is null)
            {
                throw new NotFoundException("Refund", request.RefundId);
            }

            var currentStatusCode = LookupValueHelper.GetCode(refund.RefundStatus);

            if (currentStatusCode is RefundStatusCodes.Paid or RefundStatusCodes.Rejected)
            {
                throw new ConflictException("Paid or rejected refunds cannot be rejected again.");
            }

            var rejectedStatus = await GetRefundStatusByCodeAsync(
                RefundStatusCodes.Rejected,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            refund.RefundStatusId = rejectedStatus.Id;
            refund.RejectedByUserId = _currentUserService.UserId.Value;
            refund.RejectedAt = now;
            refund.RejectionReason = request.RejectionReason.Trim();
            refund.UpdatedAt = now;
            refund.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedRefund = await LoadRefundAsync(refund.Id, cancellationToken);

            return RefundResponseMapper.Map(savedRefund);
        }

        private async Task<Dobi.Domain.Payments.RefundStatus> GetRefundStatusByCodeAsync(
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

        private async Task<Dobi.Domain.Payments.Refund> LoadRefundAsync(
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
