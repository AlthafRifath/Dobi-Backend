using Dobi.Application.Features.Reports.GetDailyRevenueReport;
using Dobi.Application.Features.Reports.GetDashboardSummary;
using Dobi.Application.Features.Reports.GetPendingChequeReport;
using Dobi.Application.Features.Reports.GetPlantWorkloadReport;
using Dobi.Application.Features.Reports.GetReadyForCollectionReport;
using Dobi.Application.Features.Reports.GetRefundReport;
using Dobi.Application.Features.Reports.GetUnpaidOrdersReport;
using Dobi.Contracts.Common;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = ReportReadRoles)]
public sealed class ReportsController : ControllerBase
{
    private const string ReportReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard-summary")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryResponse>>> GetDashboardSummary(
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetDashboardSummaryQuery(fromDate, toDate),
            cancellationToken);

        return Ok(ApiResponse<DashboardSummaryResponse>.Ok(
            response,
            "Dashboard summary loaded successfully."));
    }

    [HttpGet("daily-revenue")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<DailyRevenueReportResponse>>>> GetDailyRevenue(
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetDailyRevenueReportQuery(fromDate, toDate),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<DailyRevenueReportResponse>>.Ok(
            response,
            "Daily revenue report loaded successfully."));
    }

    [HttpGet("unpaid-orders")]
    public async Task<ActionResult<ApiResponse<PagedResponse<UnpaidOrderReportResponse>>>> GetUnpaidOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetUnpaidOrdersReportQuery(pageNumber, pageSize, searchTerm),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<UnpaidOrderReportResponse>>.Ok(
            response,
            "Unpaid orders report loaded successfully."));
    }

    [HttpGet("ready-for-collection")]
    public async Task<ActionResult<ApiResponse<PagedResponse<ReadyForCollectionReportResponse>>>> GetReadyForCollection(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetReadyForCollectionReportQuery(pageNumber, pageSize, searchTerm),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<ReadyForCollectionReportResponse>>.Ok(
            response,
            "Ready for collection report loaded successfully."));
    }

    [HttpGet("pending-cheques")]
    public async Task<ActionResult<ApiResponse<PagedResponse<PendingChequeReportResponse>>>> GetPendingCheques(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetPendingChequeReportQuery(pageNumber, pageSize, searchTerm),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<PendingChequeReportResponse>>.Ok(
            response,
            "Pending cheque report loaded successfully."));
    }

    [HttpGet("refunds")]
    public async Task<ActionResult<ApiResponse<PagedResponse<RefundReportResponse>>>> GetRefundReport(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? refundStatusId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetRefundReportQuery(
                pageNumber,
                pageSize,
                searchTerm,
                refundStatusId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<RefundReportResponse>>.Ok(
            response,
            "Refund report loaded successfully."));
    }

    [HttpGet("plant-workload")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<PlantWorkloadReportResponse>>>> GetPlantWorkload(
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetPlantWorkloadReportQuery(fromDate, toDate),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<PlantWorkloadReportResponse>>.Ok(
            response,
            "Plant workload report loaded successfully."));
    }
}