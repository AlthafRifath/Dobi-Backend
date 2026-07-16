using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetRefundReport
{
    public sealed record GetRefundReportQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? RefundStatusId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<RefundReportResponse>>;
}
