using Dobi.Application.Features.Users.CreateUser;
using Dobi.Application.Features.Users.GetUserById;
using Dobi.Application.Features.Users.GetUsers;
using Dobi.Application.Features.Users.UpdateUserStatus;
using Dobi.Contracts.Common;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateUserRequest = Dobi.Contracts.Auth.CreateUserRequest;
using UpdateUserStatusRequest = Dobi.Contracts.Auth.UpdateUserStatusRequest;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = RoleCodes.Admin)]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<UserResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PagedResponse<UserResponse>>>> GetUsers(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] string[]? roleCodes = null,
    [FromQuery] bool? isActive = null,
    [FromQuery] int? branchId = null,
    [FromQuery] int? plantId = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetUsersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                roleCodes ?? Array.Empty<string>(),
                isActive,
                branchId,
                plantId),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<UserResponse>>.Ok(
            response,
            "Users loaded successfully."));
    }

    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetUserByIdQuery(userId),
            cancellationToken);

        return Ok(ApiResponse<UserResponse>.Ok(
            response,
            "User loaded successfully."));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateUserCommand(
                request.FullName,
                request.UserName,
                request.Email,
                request.PhoneNumber,
                request.Password,
                request.Roles,
                request.DefaultBranchId,
                request.DefaultPlantId),
            cancellationToken);

        return Ok(ApiResponse<UserResponse>.Ok(
            response,
            "User created successfully."));
    }

    [HttpPatch("{userId:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUserStatus(
        [FromRoute] int userId,
        [FromBody] UpdateUserStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateUserStatusCommand(
                userId,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<UserResponse>.Ok(
            response,
            request.IsActive
                ? "User activated successfully."
                : "User deactivated successfully."));
    }
}