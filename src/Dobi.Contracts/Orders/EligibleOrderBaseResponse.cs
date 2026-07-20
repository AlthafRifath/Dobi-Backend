using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public abstract class EligibleOrderBaseResponse
    {
        public int OrderId { get; init; }
        public string OrderNo { get; init; } = string.Empty;
        public DateTime OrderDate { get; init; }

        public int CustomerId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public string? CustomerMobileNo { get; init; }

        public int CustomerTypeId { get; init; }
        public string CustomerTypeCode { get; init; } = string.Empty;
        public string CustomerTypeName { get; init; } = string.Empty;

        public int CurrentOrderStatusId { get; init; }
        public string CurrentOrderStatusCode { get; init; } = string.Empty;
        public string CurrentOrderStatusName { get; init; } = string.Empty;

        public int PaymentStatusId { get; init; }
        public string PaymentStatusCode { get; init; } = string.Empty;
        public string PaymentStatusName { get; init; } = string.Empty;

        public int ItemCount { get; init; }
        public decimal TotalQuantity { get; init; }
        public decimal TotalAmount { get; init; }
    }
}
