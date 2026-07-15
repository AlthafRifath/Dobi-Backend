using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Transfers;
using Dobi.Domain.Transfers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers
{
    internal static class TransferResponseMapper
    {
        public static async Task<TransferBatchResponse> MapAsync(
            IDobiDbContext dbContext,
            TransferBatch transferBatch,
            CancellationToken cancellationToken)
        {
            var transferType = await dbContext.TransferTypes
                .AsNoTracking()
                .FirstAsync(x => x.Id == transferBatch.TransferTypeId, cancellationToken);

            var transferStatus = await dbContext.TransferStatuses
                .AsNoTracking()
                .FirstAsync(x => x.Id == transferBatch.TransferStatusId, cancellationToken);

            var items = await dbContext.TransferBatchItems
                .AsNoTracking()
                .Include(x => x.Order)
                .Where(x => x.TransferBatchId == transferBatch.Id)
                .OrderBy(x => x.Id)
                .Select(x => new TransferBatchItemResponse(
                    x.Id,
                    x.OrderId,
                    x.Order.OrderNo,
                    x.NoOfBags,
                    x.NoOfPieces,
                    x.Remarks))
                .ToArrayAsync(cancellationToken);

            var acknowledgements = await dbContext.TransferAcknowledgements
                .AsNoTracking()
                .Include(x => x.AcknowledgementType)
                .Where(x => x.TransferBatchId == transferBatch.Id)
                .OrderBy(x => x.AcknowledgedAt)
                .ToArrayAsync(cancellationToken);

            var acknowledgementResponses = acknowledgements
                .Select(x => new TransferAcknowledgementResponse(
                    x.Id,
                    x.AcknowledgementTypeId,
                    LookupValueHelper.GetName(x.AcknowledgementType),
                    x.UserId,
                    x.AcknowledgedAt,
                    x.SignatureUrl,
                    x.Remarks))
                .ToArray();

            return new TransferBatchResponse(
                transferBatch.Id,
                transferBatch.TransferNo,
                transferBatch.TransferTypeId,
                LookupValueHelper.GetName(transferType),
                transferBatch.TransferStatusId,
                LookupValueHelper.GetName(transferStatus),
                transferBatch.FromBranchId,
                transferBatch.FromPlantId,
                transferBatch.ToBranchId,
                transferBatch.ToPlantId,
                transferBatch.DriverUserId,
                transferBatch.SentAt,
                transferBatch.ReceivedAt,
                transferBatch.Remarks,
                items,
                acknowledgementResponses);
        }
    }
}
