using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Orders;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrderById
{
    public sealed class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetOrderByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderResponse> Handle(
            GetOrderByIdQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
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
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

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
                        x.Tags.Select(t => t.TagNo).ToArray()))
                    .ToArray());
        }
    }
}
