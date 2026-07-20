using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed class EligibleRefundOrderResponse : EligibleOrderBaseResponse
    {
        public int BranchId { get; init; }
        public string BranchName { get; init; } = string.Empty;

        public decimal OrderTotalAmount { get; init; }

        public decimal ClearedPaidAmount { get; init; }

        public decimal PendingRefundAmount { get; init; }
        public decimal ApprovedPendingRefundAmount { get; init; }
        public decimal CompletedRefundAmount { get; init; }

        public decimal ReservedRefundAmount { get; init; }
        public decimal AvailableRefundAmount { get; init; }

        public bool HasConfirmedCompanyIssue { get; init; }
    }
}
