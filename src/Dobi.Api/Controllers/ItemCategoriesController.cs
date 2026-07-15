using Dobi.Application.Features.ItemCategories.CreateItemCategory;
using Dobi.Application.Features.ItemCategories.GetItemCategories;
using Dobi.Application.Features.ItemCategories.GetItemCategoryById;
using Dobi.Application.Features.ItemCategories.UpdateItemCategory;
using Dobi.Application.Features.ItemCategories.UpdateItemCategoryStatus;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/item-categories")]
[Authorize]
public sealed class ItemCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<ItemCategoryResponse>>>> GetItemCategories(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? pricingTypeId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetItemCategoriesQuery(pageNumber, pageSize, searchTerm, pricingTypeId, isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<ItemCategoryResponse>>.Ok(response, "Item categories loaded successfully."));
    }

    [HttpGet("{itemCategoryId:int}")]
    public async Task<ActionResult<ApiResponse<ItemCategoryResponse>>> GetItemCategoryById(
        [FromRoute] int itemCategoryId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetItemCategoryByIdQuery(itemCategoryId),
            cancellationToken);

        return Ok(ApiResponse<ItemCategoryResponse>.Ok(response, "Item category loaded successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ItemCategoryResponse>>> CreateItemCategory(
        [FromBody] ItemCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateItemCategoryCommand(
                request.CategoryName,
                request.DefaultPricingTypeId,
                request.IsSpecialItem,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ItemCategoryResponse>.Ok(response, "Item category created successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPut("{itemCategoryId:int}")]
    public async Task<ActionResult<ApiResponse<ItemCategoryResponse>>> UpdateItemCategory(
        [FromRoute] int itemCategoryId,
        [FromBody] ItemCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateItemCategoryCommand(
                itemCategoryId,
                request.CategoryName,
                request.DefaultPricingTypeId,
                request.IsSpecialItem,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ItemCategoryResponse>.Ok(response, "Item category updated successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPatch("{itemCategoryId:int}/status")]
    public async Task<ActionResult<ApiResponse<ItemCategoryResponse>>> UpdateItemCategoryStatus(
        [FromRoute] int itemCategoryId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateItemCategoryStatusCommand(itemCategoryId, request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ItemCategoryResponse>.Ok(
            response,
            request.IsActive ? "Item category activated successfully." : "Item category deactivated successfully."));
    }
}