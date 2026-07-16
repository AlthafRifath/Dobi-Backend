using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetPendingChequeReport
{
    public sealed record GetPendingChequeReportQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm) : IRequest<PagedResponse<PendingChequeReportResponse>>;
}
