using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Payments;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPaymentById
{
    public sealed class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, PaymentResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPaymentByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentResponse> Handle(
            GetPaymentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var payment = await _dbContext.Payments
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.PaymentMethod)
                .Include(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.PaymentId, cancellationToken);

            if (payment is null)
            {
                throw new NotFoundException("Payment", request.PaymentId);
            }

            var summary = await PaymentStatusUpdater.GetSummaryAsync(
                _dbContext,
                payment.Order,
                cancellationToken);

            return PaymentResponseMapper.Map(payment, summary);
        }
    }
}
