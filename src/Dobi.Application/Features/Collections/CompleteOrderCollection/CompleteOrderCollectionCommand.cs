using Dobi.Contracts.Collections;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.CompleteOrderCollection
{
    public sealed record CompleteOrderCollectionCommand(
        int OrderId,
        bool IsCollectedByCustomer,
        string? CollectorName,
        string? CollectorMobileNo,
        bool ReceiptVerified,
        bool MobileNoVerified,
        string? ReceiptImageUrl,
        string? CustomerSignatureUrl,
        string? Remarks) : IRequest<OrderCollectionResponse>;
}
