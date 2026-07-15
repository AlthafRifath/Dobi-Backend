using Dobi.Contracts.Transfers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.GetTransferById
{
    public sealed record GetTransferByIdQuery(
        int TransferBatchId) : IRequest<TransferBatchResponse>;
}
