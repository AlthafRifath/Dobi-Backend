using Dobi.Contracts.Transfers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.CreateTransferBatch
{
    public sealed record CreateTransferBatchCommand(
        int TransferTypeId,
        int? FromBranchId,
        int? FromPlantId,
        int? ToBranchId,
        int? ToPlantId,
        int DriverUserId,
        IReadOnlyCollection<CreateTransferBatchItemRequest> Items,
        string? Remarks) : IRequest<TransferBatchResponse>;
}
