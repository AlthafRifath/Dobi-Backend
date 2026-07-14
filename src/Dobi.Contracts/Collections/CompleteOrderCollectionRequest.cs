using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Collections
{
    public sealed record CompleteOrderCollectionRequest(
        int CollectionModeId,
        bool IsCollectedByCustomer,
        string? CollectorName,
        string? CollectorMobileNo,
        bool ReceiptVerified,
        bool MobileNoVerified,
        string? ReceiptImageUrl,
        string? CustomerSignatureUrl,
        DateTime CollectedOrDeliveredAt,
        string? Remarks);
}
