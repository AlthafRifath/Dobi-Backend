using Dobi.Contracts.Common;
using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.GetRefunds
{
    public sealed record GetRefundsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? OrderId,
        int? PaymentId,
        int? RefundStatusId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<RefundResponse>>;
}
