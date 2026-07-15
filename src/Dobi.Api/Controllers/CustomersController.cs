using Dobi.Application.Features.Customers.CreateCustomer;
using Dobi.Application.Features.Customers.GetCustomerById;
using Dobi.Application.Features.Customers.GetCustomers;
using Dobi.Application.Features.Customers.UpdateCustomer;
using Dobi.Application.Features.Customers.UpdateCustomerStatus;
using Dobi.Contracts.Common;
using Dobi.Contracts.Customers;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = CustomerReadRoles)]
public sealed class CustomersController : ControllerBase
{
    private const string CustomerReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string CustomerWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager;

    private const string CustomerStatusRoles =
        RoleCodes.Admin + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<CustomerResponse>>>> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? customerTypeId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetCustomersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                customerTypeId,
                isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<CustomerResponse>>.Ok(
            response,
            "Customers loaded successfully."));
    }

    [HttpGet("{customerId:int}")]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> GetCustomerById(
        [FromRoute] int customerId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetCustomerByIdQuery(customerId),
            cancellationToken);

        return Ok(ApiResponse<CustomerResponse>.Ok(
            response,
            "Customer loaded successfully."));
    }

    [Authorize(Roles = CustomerWriteRoles)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateCustomerCommand(
                request.FullName,
                request.MobileNo,
                request.Address,
                request.Email,
                request.CustomerTypeId),
            cancellationToken);

        return Ok(ApiResponse<CustomerResponse>.Ok(
            response,
            "Customer created successfully."));
    }

    [Authorize(Roles = CustomerWriteRoles)]
    [HttpPut("{customerId:int}")]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> UpdateCustomer(
        [FromRoute] int customerId,
        [FromBody] UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateCustomerCommand(
                customerId,
                request.FullName,
                request.MobileNo,
                request.Address,
                request.Email,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<CustomerResponse>.Ok(
            response,
            "Customer updated successfully."));
    }

    [Authorize(Roles = CustomerStatusRoles)]
    [HttpPatch("{customerId:int}/status")]
    public async Task<ActionResult<ApiResponse<CustomerResponse>>> UpdateCustomerStatus(
        [FromRoute] int customerId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateCustomerStatusCommand(
                customerId,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<CustomerResponse>.Ok(
            response,
            request.IsActive
                ? "Customer activated successfully."
                : "Customer deactivated successfully."));
    }
}