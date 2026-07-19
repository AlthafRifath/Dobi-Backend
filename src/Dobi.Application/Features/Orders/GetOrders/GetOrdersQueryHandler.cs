using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Orders.GetOrders;

public sealed class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, PagedResponse<OrderResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetOrdersQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<OrderResponse>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Branch)
            .Include(x => x.CurrentStatus)
            .Include(x => x.PaymentStatus)
            .Include(x => x.Items)
                .ThenInclude(x => x.Service)
            .Include(x => x.Items)
                .ThenInclude(x => x.ItemCategory)
            .Include(x => x.Items)
                .ThenInclude(x => x.PricingType)
            .Include(x => x.Items)
                .ThenInclude(x => x.Tags)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim().ToLower();

            query = query.Where(x =>
                x.OrderNo.ToLower().Contains(searchTerm) ||
                x.Customer.FullName.ToLower().Contains(searchTerm) ||
                x.Customer.MobileNo.ToLower().Contains(searchTerm));
        }

        if (request.BranchId.HasValue)
        {
            query = query.Where(x =>
                x.BranchId == request.BranchId.Value);
        }

        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId.Value);
        }

        if (request.StatusId.HasValue)
        {
            query = query.Where(x =>
                x.CurrentStatusId == request.StatusId.Value);
        }

        if (request.PaymentStatusId.HasValue)
        {
            query = query.Where(x =>
                x.PaymentStatusId == request.PaymentStatusId.Value);
        }

        if (request.FromDate.HasValue)
        {
            var fromDateUtc = request.FromDate.Value.ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc);

            query = query.Where(x =>
                x.OrderDate >= fromDateUtc);
        }

        if (request.ToDate.HasValue)
        {
            var toDateExclusiveUtc = request.ToDate.Value
                .AddDays(1)
                .ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc);

            query = query.Where(x =>
                x.OrderDate < toDateExclusiveUtc);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(x => x.OrderDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var mappedOrders = orders
            .Select(MapOrderResponse)
            .ToArray();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<OrderResponse>(
            mappedOrders,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }

    private static OrderResponse MapOrderResponse(
        Dobi.Domain.Orders.Order order)
    {
        return new OrderResponse(
            order.Id,
            order.OrderNo,
            order.CustomerId,
            order.Customer.FullName,
            order.Customer.MobileNo,
            order.BranchId,
            order.Branch.BranchName,
            order.OrderDate,
            order.ExpectedReturnDate,
            order.CurrentStatusId,
            order.CurrentStatus.StatusName,
            order.PaymentStatusId,
            order.PaymentStatus.StatusName,
            order.IsExpress,
            order.SubTotalAmount,
            order.ExpressChargeAmount,
            order.TotalAmount,
            order.Items
                .OrderBy(x => x.Id)
                .Select(x => new OrderItemResponse(
                    x.Id,
                    x.ServiceId,
                    x.Service.ServiceName,
                    x.ItemCategoryId,
                    x.ItemCategory.CategoryName,
                    x.PricingTypeId,
                    x.PricingType.PricingTypeName,
                    x.Quantity,
                    x.WeightKg,
                    x.UnitPrice,
                    x.LineAmount,
                    x.SpecialNotes,
                    x.Tags
                        .Select(t => t.TagNo)
                        .ToArray()))
                .ToArray());
    }
}