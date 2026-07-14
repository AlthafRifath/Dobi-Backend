using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record OrderResponse(
        int OrderId,
        string OrderNo,
        int CustomerId,
        string CustomerName,
        string CustomerMobileNo,
        int BranchId,
        string BranchName,
        DateTime OrderDate,
        DateOnly? ExpectedReturnDate,
        int CurrentStatusId,
        string CurrentStatusName,
        int PaymentStatusId,
        string PaymentStatusName,
        bool IsExpress,
        decimal SubTotalAmount,
        decimal ExpressChargeAmount,
        decimal TotalAmount,
        IReadOnlyCollection<OrderItemResponse> Items);
}
