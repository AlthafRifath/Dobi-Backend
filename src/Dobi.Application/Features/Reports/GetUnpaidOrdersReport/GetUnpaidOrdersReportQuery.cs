using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetUnpaidOrdersReport
{
    public sealed record GetUnpaidOrdersReportQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm) : IRequest<PagedResponse<UnpaidOrderReportResponse>>;
}
