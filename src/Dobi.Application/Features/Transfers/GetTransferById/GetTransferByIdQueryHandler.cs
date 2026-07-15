using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Transfers;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.GetTransferById
{
    public sealed class GetTransferByIdQueryHandler
    : IRequestHandler<GetTransferByIdQuery, TransferBatchResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetTransferByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransferBatchResponse> Handle(
            GetTransferByIdQuery request,
            CancellationToken cancellationToken)
        {
            var transfer = await _dbContext.TransferBatches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.TransferBatchId, cancellationToken);

            if (transfer is null)
            {
                throw new NotFoundException("Transfer batch", request.TransferBatchId);
            }

            return await TransferResponseMapper.MapAsync(
                _dbContext,
                transfer,
                cancellationToken);
        }
    }
}
