using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Collections
{
    public class OrderCollection : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int CollectionModeId { get; set; }
        public CollectionMode CollectionMode { get; set; } = null!;

        public bool IsCollectedByCustomer { get; set; }

        public string? CollectorName { get; set; }
        public string? CollectorMobileNo { get; set; }

        public bool ReceiptVerified { get; set; }
        public bool MobileNoVerified { get; set; }

        public string? ReceiptImageUrl { get; set; }
        public string? CustomerSignatureUrl { get; set; }

        public DateTime CollectedOrDeliveredAt { get; set; }

        public int ReleasedByUserId { get; set; }

        public string? Remarks { get; set; }
    }
}
