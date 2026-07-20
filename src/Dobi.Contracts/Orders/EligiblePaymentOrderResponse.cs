using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed class EligiblePaymentOrderResponse : EligibleOrderBaseResponse
    {
        public int BranchId { get; init; }
        public string BranchName { get; init; } = string.Empty;

        public int? CollectionId { get; init; }
        public int? CollectionModeId { get; init; }
        public string? CollectionModeCode { get; init; }
        public string? CollectionModeName { get; init; }
        public DateTime? CollectedOrDeliveredAt { get; init; }

        public decimal OrderTotalAmount { get; init; }
        public decimal PaidAmount { get; init; }
        public decimal PendingClearanceAmount { get; init; }
        public decimal FailedAmount { get; init; }
        public decimal OutstandingAmount { get; init; }
    }
}
