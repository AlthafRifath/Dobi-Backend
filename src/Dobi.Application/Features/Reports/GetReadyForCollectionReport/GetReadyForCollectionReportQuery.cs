using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetReadyForCollectionReport
{
    public sealed record GetReadyForCollectionReportQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm) : IRequest<PagedResponse<ReadyForCollectionReportResponse>>;
}
