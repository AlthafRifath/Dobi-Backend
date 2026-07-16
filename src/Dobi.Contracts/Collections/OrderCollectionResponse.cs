using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Collections
{
    public sealed record OrderCollectionResponse(
        int? OrderCollectionId,
        int OrderId,
        string OrderNo,

        int CustomerId,
        string CustomerName,
        string CustomerMobileNo,
        string CustomerAddress,
        int CustomerTypeId,
        string CustomerTypeCode,
        string CustomerTypeName,

        int BranchId,
        string BranchName,

        int? CollectionModeId,
        string? CollectionModeName,

        bool? IsCollectedByCustomer,
        string? CollectorName,
        string? CollectorMobileNo,

        bool? ReceiptVerified,
        bool? MobileNoVerified,

        string? ReceiptImageUrl,
        string? CustomerSignatureUrl,

        DateTime? CollectedOrDeliveredAt,
        int? ReleasedByUserId,

        string? Remarks,

        int CurrentOrderStatusId,
        string CurrentOrderStatusName,

        int PaymentStatusId,
        string PaymentStatusName,

        decimal TotalAmount);
}
