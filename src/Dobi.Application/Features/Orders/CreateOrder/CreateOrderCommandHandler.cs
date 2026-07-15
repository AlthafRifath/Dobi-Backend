using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Orders;
using Dobi.Domain.Inspections;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateOrderCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<OrderResponse> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var customer = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException("Customer", request.CustomerId);
            }

            if (!customer.IsActive)
            {
                throw new BadRequestException("Selected customer is inactive.");
            }

            var branch = await _dbContext.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.BranchId, cancellationToken);

            if (branch is null)
            {
                throw new NotFoundException("Branch", request.BranchId);
            }

            if (!branch.IsActive)
            {
                throw new BadRequestException("Selected branch is inactive.");
            }

            var createdStatus = await _dbContext.OrderStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StatusCode == OrderStatusCodes.Created, cancellationToken);

            if (createdStatus is null)
            {
                throw new InvalidOperationException("Created order status was not found.");
            }

            var unpaidPaymentStatus = await _dbContext.PaymentStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StatusCode == PaymentStatusCodes.Unpaid, cancellationToken);

            if (unpaidPaymentStatus is null)
            {
                throw new InvalidOperationException("Unpaid payment status was not found.");
            }

            var orderItems = new List<OrderItem>();
            var orderItemResponses = new List<OrderItemResponse>();

            decimal subTotalAmount = 0;
            decimal expressChargeAmount = 0;

            var itemIndex = 0;

            foreach (var itemRequest in request.Items)
            {
                var service = await _dbContext.Services
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == itemRequest.ServiceId, cancellationToken);

                if (service is null)
                {
                    throw new NotFoundException("Service", itemRequest.ServiceId);
                }

                if (!service.IsActive)
                {
                    throw new BadRequestException($"Service '{service.ServiceName}' is inactive.");
                }

                if (request.IsExpress && !service.IsExpressEligible)
                {
                    throw new BadRequestException($"Service '{service.ServiceName}' is not express eligible.");
                }

                var itemCategory = await _dbContext.ItemCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == itemRequest.ItemCategoryId, cancellationToken);

                if (itemCategory is null)
                {
                    throw new NotFoundException("Item category", itemRequest.ItemCategoryId);
                }

                if (!itemCategory.IsActive)
                {
                    throw new BadRequestException($"Item category '{itemCategory.CategoryName}' is inactive.");
                }

                var pricingType = await _dbContext.PricingTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == itemRequest.PricingTypeId, cancellationToken);

                if (pricingType is null)
                {
                    throw new NotFoundException("Pricing type", itemRequest.PricingTypeId);
                }

                var today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow);

                var servicePriceQuery = _dbContext.ServicePrices
                    .AsNoTracking()
                    .Where(x =>
                        x.ServiceId == itemRequest.ServiceId &&
                        x.ItemCategoryId == itemRequest.ItemCategoryId &&
                        x.PricingTypeId == itemRequest.PricingTypeId &&
                        x.IsActive &&
                        x.EffectiveFrom <= today &&
                        (x.EffectiveTo == null || x.EffectiveTo >= today));

                if (itemRequest.ServicePriceId.HasValue)
                {
                    servicePriceQuery = servicePriceQuery.Where(x => x.Id == itemRequest.ServicePriceId.Value);
                }

                var servicePrice = await servicePriceQuery
                    .OrderByDescending(x => x.EffectiveFrom)
                    .FirstOrDefaultAsync(cancellationToken);

                if (servicePrice is null)
                {
                    throw new BadRequestException(
                        $"No active service price found for service '{service.ServiceName}' and item category '{itemCategory.CategoryName}'.");
                }

                decimal quantityForCalculation;

                if (pricingType.PricingTypeCode == PricingTypeCodes.PerKg)
                {
                    if (!itemRequest.WeightKg.HasValue || itemRequest.WeightKg.Value <= 0)
                    {
                        throw new BadRequestException("Weight is required for per-kg pricing.");
                    }

                    quantityForCalculation = itemRequest.WeightKg.Value;
                }
                else if (pricingType.PricingTypeCode == PricingTypeCodes.PerItem)
                {
                    quantityForCalculation = itemRequest.Quantity;
                }
                else
                {
                    throw new BadRequestException($"Unsupported pricing type '{pricingType.PricingTypeCode}'.");
                }

                var unitPrice = servicePrice.BasePrice;
                var lineAmount = unitPrice * quantityForCalculation;

                var lineExpressCharge = request.IsExpress
                    ? (servicePrice.ExpressAdditionalPrice ?? 0) * quantityForCalculation
                    : 0;

                subTotalAmount += lineAmount;
                expressChargeAmount += lineExpressCharge;

                var orderItem = new OrderItem
                {
                    ServiceId = itemRequest.ServiceId,
                    ItemCategoryId = itemRequest.ItemCategoryId,
                    PricingTypeId = itemRequest.PricingTypeId,
                    ServicePriceId = servicePrice.Id,
                    Quantity = itemRequest.Quantity,
                    WeightKg = itemRequest.WeightKg,
                    UnitPrice = unitPrice,
                    LineAmount = lineAmount,
                    SpecialNotes = string.IsNullOrWhiteSpace(itemRequest.SpecialNotes)
                        ? null
                        : itemRequest.SpecialNotes.Trim()
                };

                if (itemRequest.ManualTagNumbers is not null)
                {
                    foreach (var tagNumber in itemRequest.ManualTagNumbers
                                 .Where(x => !string.IsNullOrWhiteSpace(x))
                                 .Select(x => x.Trim())
                                 .Distinct())
                    {
                        var tagExists = await _dbContext.OrderItemTags.AnyAsync(
                            x => x.TagNo == tagNumber,
                            cancellationToken);

                        if (tagExists)
                        {
                            throw new ConflictException($"Manual tag number '{tagNumber}' already exists.");
                        }

                        orderItem.Tags.Add(new OrderItemTag
                        {
                            TagNo = tagNumber
                        });
                    }
                }

                orderItems.Add(orderItem);

                orderItemResponses.Add(new OrderItemResponse(
                    0,
                    service.Id,
                    service.ServiceName,
                    itemCategory.Id,
                    itemCategory.CategoryName,
                    pricingType.Id,
                    pricingType.PricingTypeName,
                    itemRequest.Quantity,
                    itemRequest.WeightKg,
                    unitPrice,
                    lineAmount,
                    orderItem.SpecialNotes,
                    itemRequest.ManualTagNumbers?.ToArray() ?? Array.Empty<string>()));

                itemIndex++;
            }

            var order = new Order
            {
                OrderNo = $"TMP-{Guid.NewGuid():N}"[..30],
                CustomerId = request.CustomerId,
                BranchId = request.BranchId,
                OrderDate = _dateTimeProvider.UtcNow,
                ExpectedReturnDate = request.ExpectedReturnDate,
                CurrentStatusId = createdStatus.Id,
                PaymentStatusId = unpaidPaymentStatus.Id,
                IsExpress = request.IsExpress,
                SubTotalAmount = subTotalAmount,
                ExpressChargeAmount = expressChargeAmount,
                TotalAmount = subTotalAmount + expressChargeAmount,
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            foreach (var orderItem in orderItems)
            {
                order.Items.Add(orderItem);
            }

            _dbContext.Orders.Add(order);

            await _dbContext.SaveChangesAsync(cancellationToken);

            order.OrderNo = $"ORD-{order.Id:D6}";

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = createdStatus.Id,
                Remarks = "Order created.",
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = _dateTimeProvider.UtcNow
            });

            if (request.Inspections is not null && request.Inspections.Any())
            {
                await CreateInspectionRecordsAsync(
                    order,
                    request.Inspections,
                    cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedOrder = await _dbContext.Orders
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
                .FirstAsync(x => x.Id == order.Id, cancellationToken);

            return MapOrderResponse(savedOrder);
        }

        private async Task CreateInspectionRecordsAsync(
            Order order,
            IReadOnlyCollection<CreateOrderInspectionRequest> inspections,
            CancellationToken cancellationToken)
        {
            foreach (var inspectionRequest in inspections)
            {
                int? orderItemId = null;

                if (inspectionRequest.OrderItemTempIndex.HasValue)
                {
                    var itemIndex = inspectionRequest.OrderItemTempIndex.Value;

                    if (itemIndex < 0 || itemIndex >= order.Items.Count)
                    {
                        throw new BadRequestException($"Invalid order item index '{itemIndex}' for inspection.");
                    }

                    orderItemId = order.Items.ElementAt(itemIndex).Id;
                }

                foreach (var issueTypeId in inspectionRequest.InspectionIssueTypeIds.Distinct())
                {
                    var issueTypeExists = await _dbContext.InspectionIssueTypes.AnyAsync(
                        x => x.Id == issueTypeId,
                        cancellationToken);

                    if (!issueTypeExists)
                    {
                        throw new NotFoundException("Inspection issue type", issueTypeId);
                    }
                }

                var inspectionRecord = new InspectionRecord
                {
                    OrderId = order.Id,
                    OrderItemId = orderItemId,
                    CustomerAcknowledged = inspectionRequest.CustomerAcknowledged,
                    CustomerSignatureUrl = string.IsNullOrWhiteSpace(inspectionRequest.CustomerSignatureUrl)
                        ? null
                        : inspectionRequest.CustomerSignatureUrl.Trim(),
                    Notes = string.IsNullOrWhiteSpace(inspectionRequest.Notes)
                        ? null
                        : inspectionRequest.Notes.Trim(),
                    InspectedByUserId = _currentUserService.UserId!.Value,
                    InspectedAt = _dateTimeProvider.UtcNow
                };

                foreach (var issueTypeId in inspectionRequest.InspectionIssueTypeIds.Distinct())
                {
                    inspectionRecord.Issues.Add(new InspectionIssue
                    {
                        InspectionIssueTypeId = issueTypeId,
                        Notes = null
                    });
                }

                if (inspectionRequest.PhotoUrls is not null)
                {
                    foreach (var photoUrl in inspectionRequest.PhotoUrls
                                 .Where(x => !string.IsNullOrWhiteSpace(x))
                                 .Select(x => x.Trim())
                                 .Distinct())
                    {
                        inspectionRecord.Photos.Add(new InspectionPhoto
                        {
                            PhotoUrl = photoUrl,
                            UploadedByUserId = _currentUserService.UserId.Value,
                            UploadedAt = _dateTimeProvider.UtcNow
                        });
                    }
                }

                _dbContext.InspectionRecords.Add(inspectionRecord);
            }
        }

        private static OrderResponse MapOrderResponse(Order order)
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
                        x.Tags.Select(t => t.TagNo).ToArray()))
                    .ToArray());
        }
    }
}
