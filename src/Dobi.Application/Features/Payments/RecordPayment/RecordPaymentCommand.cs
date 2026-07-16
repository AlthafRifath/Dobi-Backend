using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.RecordPayment
{
    public sealed record RecordPaymentCommand(
        int OrderId,
        decimal Amount,
        int PaymentMethodId,
        string? ReferenceNo,
        string? ChequeNo,
        string? ChequeBankName,
        DateOnly? ChequeDate) : IRequest<PaymentResponse>;
}
