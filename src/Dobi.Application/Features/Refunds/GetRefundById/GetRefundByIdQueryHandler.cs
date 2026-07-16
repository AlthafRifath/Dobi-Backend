using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Refunds;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.GetRefundById
{
    public sealed class GetRefundByIdQueryHandler
    : IRequestHandler<GetRefundByIdQuery, RefundResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetRefundByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefundResponse> Handle(
            GetRefundByIdQuery request,
            CancellationToken cancellationToken)
        {
            var refund = await _dbContext.Refunds
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.RefundStatus)
                .FirstOrDefaultAsync(x => x.Id == request.RefundId, cancellationToken);

            if (refund is null)
            {
                throw new NotFoundException("Refund", request.RefundId);
            }

            return RefundResponseMapper.Map(refund);
        }
    }
}
