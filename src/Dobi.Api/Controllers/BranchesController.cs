using Dobi.Application.Features.Branches.CreateBranch;
using Dobi.Application.Features.Branches.GetBranchById;
using Dobi.Application.Features.Branches.GetBranches;
using Dobi.Application.Features.Branches.UpdateBranch;
using Dobi.Application.Features.Branches.UpdateBranchStatus;
using Dobi.Contracts.Branches;
using Dobi.Contracts.Common;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/branches")]
[Authorize]
public sealed class BranchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<BranchResponse>>>> GetBranches(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetBranchesQuery(
                pageNumber,
                pageSize,
                searchTerm,
                isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<BranchResponse>>.Ok(
            response,
            "Branches loaded successfully."));
    }

    [HttpGet("{branchId:int}")]
    public async Task<ActionResult<ApiResponse<BranchResponse>>> GetBranchById(
        [FromRoute] int branchId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetBranchByIdQuery(branchId),
            cancellationToken);

        return Ok(ApiResponse<BranchResponse>.Ok(
            response,
            "Branch loaded successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BranchResponse>>> CreateBranch(
        [FromBody] BranchRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateBranchCommand(
                request.BranchName,
                request.Address,
                request.ContactNo,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<BranchResponse>.Ok(
            response,
            "Branch created successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPut("{branchId:int}")]
    public async Task<ActionResult<ApiResponse<BranchResponse>>> UpdateBranch(
        [FromRoute] int branchId,
        [FromBody] BranchRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateBranchCommand(
                branchId,
                request.BranchName,
                request.Address,
                request.ContactNo,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<BranchResponse>.Ok(
            response,
            "Branch updated successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPatch("{branchId:int}/status")]
    public async Task<ActionResult<ApiResponse<BranchResponse>>> UpdateBranchStatus(
        [FromRoute] int branchId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateBranchStatusCommand(
                branchId,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<BranchResponse>.Ok(
            response,
            request.IsActive
                ? "Branch activated successfully."
                : "Branch deactivated successfully."));
    }
}