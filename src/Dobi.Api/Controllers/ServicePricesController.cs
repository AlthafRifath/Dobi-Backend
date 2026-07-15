using Dobi.Application.Features.ServicePrices.CreateServicePrice;
using Dobi.Application.Features.ServicePrices.GetServicePriceById;
using Dobi.Application.Features.ServicePrices.GetServicePrices;
using Dobi.Application.Features.ServicePrices.UpdateServicePrice;
using Dobi.Application.Features.ServicePrices.UpdateServicePriceStatus;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/service-prices")]
[Authorize]
public sealed class ServicePricesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicePricesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<ServicePriceResponse>>>> GetServicePrices(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? serviceId = null,
        [FromQuery] int? itemCategoryId = null,
        [FromQuery] int? pricingTypeId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetServicePricesQuery(pageNumber, pageSize, serviceId, itemCategoryId, pricingTypeId, isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<ServicePriceResponse>>.Ok(response, "Service prices loaded successfully."));
    }

    [HttpGet("{servicePriceId:int}")]
    public async Task<ActionResult<ApiResponse<ServicePriceResponse>>> GetServicePriceById(
        [FromRoute] int servicePriceId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetServicePriceByIdQuery(servicePriceId),
            cancellationToken);

        return Ok(ApiResponse<ServicePriceResponse>.Ok(response, "Service price loaded successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ServicePriceResponse>>> CreateServicePrice(
        [FromBody] ServicePriceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateServicePriceCommand(
                request.ServiceId,
                request.ItemCategoryId,
                request.PricingTypeId,
                request.BasePrice,
                request.ExpressAdditionalPrice,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServicePriceResponse>.Ok(response, "Service price created successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPut("{servicePriceId:int}")]
    public async Task<ActionResult<ApiResponse<ServicePriceResponse>>> UpdateServicePrice(
        [FromRoute] int servicePriceId,
        [FromBody] ServicePriceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateServicePriceCommand(
                servicePriceId,
                request.ServiceId,
                request.ItemCategoryId,
                request.PricingTypeId,
                request.BasePrice,
                request.ExpressAdditionalPrice,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServicePriceResponse>.Ok(response, "Service price updated successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPatch("{servicePriceId:int}/status")]
    public async Task<ActionResult<ApiResponse<ServicePriceResponse>>> UpdateServicePriceStatus(
        [FromRoute] int servicePriceId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateServicePriceStatusCommand(servicePriceId, request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServicePriceResponse>.Ok(
            response,
            request.IsActive ? "Service price activated successfully." : "Service price deactivated successfully."));
    }
}