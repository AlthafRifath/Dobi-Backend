using Dobi.Application.Common;
using Dobi.Contracts.Collections;
using Dobi.Domain.Collections;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections
{
    internal static class OrderCollectionResponseMapper
    {
        public static OrderCollectionResponse Map(Order order)
        {
            var collection = order.Collection;

            return new OrderCollectionResponse(
                collection?.Id,
                order.Id,
                order.OrderNo,

                order.CustomerId,
                order.Customer.FullName,
                order.Customer.MobileNo,
                order.Customer.Address,
                order.Customer.CustomerTypeId,
                LookupValueHelper.GetCode(order.Customer.CustomerType),
                LookupValueHelper.GetName(order.Customer.CustomerType),

                order.BranchId,
                order.Branch.BranchName,

                collection?.CollectionModeId,
                collection?.CollectionMode is null
                    ? null
                    : LookupValueHelper.GetName(collection.CollectionMode),

                collection?.IsCollectedByCustomer,
                collection?.CollectorName,
                collection?.CollectorMobileNo,

                collection?.ReceiptVerified,
                collection?.MobileNoVerified,

                collection?.ReceiptImageUrl,
                collection?.CustomerSignatureUrl,

                collection?.CollectedOrDeliveredAt,
                collection?.ReleasedByUserId,

                collection?.Remarks,

                order.CurrentStatusId,
                LookupValueHelper.GetName(order.CurrentStatus),

                order.PaymentStatusId,
                LookupValueHelper.GetName(order.PaymentStatus),

                order.TotalAmount);
        }

        public static OrderCollectionResponse Map(OrderCollection collection)
        {
            var order = collection.Order;

            return new OrderCollectionResponse(
                collection.Id,
                order.Id,
                order.OrderNo,

                order.CustomerId,
                order.Customer.FullName,
                order.Customer.MobileNo,
                order.Customer.Address,
                order.Customer.CustomerTypeId,
                LookupValueHelper.GetCode(order.Customer.CustomerType),
                LookupValueHelper.GetName(order.Customer.CustomerType),

                order.BranchId,
                order.Branch.BranchName,

                collection.CollectionModeId,
                LookupValueHelper.GetName(collection.CollectionMode),

                collection.IsCollectedByCustomer,
                collection.CollectorName,
                collection.CollectorMobileNo,

                collection.ReceiptVerified,
                collection.MobileNoVerified,

                collection.ReceiptImageUrl,
                collection.CustomerSignatureUrl,

                collection.CollectedOrDeliveredAt,
                collection.ReleasedByUserId,

                collection.Remarks,

                order.CurrentStatusId,
                LookupValueHelper.GetName(order.CurrentStatus),

                order.PaymentStatusId,
                LookupValueHelper.GetName(order.PaymentStatus),

                order.TotalAmount);
        }
    }
}
