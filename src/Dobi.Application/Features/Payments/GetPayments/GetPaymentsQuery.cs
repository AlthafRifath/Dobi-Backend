using Dobi.Contracts.Common;
using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPayments
{
    public sealed record GetPaymentsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? OrderId,
        int? PaymentMethodId,
        int? PaymentStatusId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<PaymentResponse>>;
}
