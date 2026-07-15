using Dobi.Contracts.Common;
using Dobi.Contracts.Transfers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.GetTransfers
{
    public sealed record GetTransfersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? TransferTypeId,
        int? TransferStatusId,
        int? DriverUserId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<TransferBatchResponse>>;
}
