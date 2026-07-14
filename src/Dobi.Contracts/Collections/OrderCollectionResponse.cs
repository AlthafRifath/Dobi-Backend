using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Collections
{
    public sealed record OrderCollectionResponse(
        int OrderCollectionId,
        int OrderId,
        string OrderNo,
        int CollectionModeId,
        string CollectionModeName,
        bool IsCollectedByCustomer,
        string? CollectorName,
        string? CollectorMobileNo,
        bool ReceiptVerified,
        bool MobileNoVerified,
        string? ReceiptImageUrl,
        string? CustomerSignatureUrl,
        DateTime CollectedOrDeliveredAt,
        int ReleasedByUserId,
        string? Remarks);
}
