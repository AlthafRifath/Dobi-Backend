using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.RecordPayment
{
    public sealed class RecordPaymentCommandValidator : AbstractValidator<RecordPaymentCommand>
    {
        public RecordPaymentCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.PaymentMethodId)
                .GreaterThan(0);

            RuleFor(x => x.ReferenceNo)
                .MaximumLength(100);

            RuleFor(x => x.ChequeNo)
                .MaximumLength(50);

            RuleFor(x => x.ChequeBankName)
                .MaximumLength(150);
        }
    }
}
