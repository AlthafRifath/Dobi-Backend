using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Constants
{
    public static class OrderStatusCodes
    {
        public const string Draft = "DRAFT";
        public const string Created = "CREATED";
        public const string Cancelled = "CANCELLED";
        public const string SentToPlant = "SENT_TO_PLANT";
        public const string ReceivedAtPlant = "RECEIVED_AT_PLANT";
        public const string Processing = "PROCESSING";
        public const string QcPending = "QC_PENDING";
        public const string QcFailed = "QC_FAILED";
        public const string Packed = "PACKED";
        public const string ReadyForOutletReturn = "READY_FOR_OUTLET_RETURN";
        public const string ReturnedToOutlet = "RETURNED_TO_OUTLET";
        public const string ReadyForCollection = "READY_FOR_COLLECTION";
        public const string CollectedDelivered = "COLLECTED_DELIVERED";
        public const string Closed = "CLOSED";
    }
}
