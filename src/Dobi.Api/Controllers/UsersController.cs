using Dobi.Application.Features.Users.CreateUser;
using Dobi.Contracts.Auth;
using Dobi.Contracts.Common;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers
{
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
    }
}
