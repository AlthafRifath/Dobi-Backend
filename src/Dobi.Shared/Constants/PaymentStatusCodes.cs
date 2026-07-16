using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Constants
{
    public static class PaymentStatusCodes
    {
        public const string Unpaid = "UNPAID";
        public const string PendingClearance = "PENDING_CLEARANCE";
        public const string PartiallyPaid = "PARTIALLY_PAID";
        public const string PartiallyPaidPendingClearance = "PARTIALLY_PAID_PENDING_CLEARANCE";
        public const string Paid = "PAID";
        public const string Refunded = "REFUNDED";
        public const string Failed = "FAILED";
    }
}
