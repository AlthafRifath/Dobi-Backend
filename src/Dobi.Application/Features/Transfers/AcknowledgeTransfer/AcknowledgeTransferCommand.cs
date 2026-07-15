using Dobi.Contracts.Transfers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.AcknowledgeTransfer
{
    public sealed record AcknowledgeTransferCommand(
        int TransferBatchId,
        int AcknowledgementTypeId,
        string? SignatureUrl,
        string? Remarks) : IRequest<TransferBatchResponse>;
}
