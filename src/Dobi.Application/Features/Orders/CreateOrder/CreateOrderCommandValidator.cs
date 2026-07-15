using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.CreateOrder
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.BranchId)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one order item is required.");

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ServiceId)
                        .GreaterThan(0);

                    item.RuleFor(x => x.ItemCategoryId)
                        .GreaterThan(0);

                    item.RuleFor(x => x.PricingTypeId)
                        .GreaterThan(0);

                    item.RuleFor(x => x.ServicePriceId)
                        .GreaterThan(0)
                        .When(x => x.ServicePriceId.HasValue);

                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0);

                    item.RuleFor(x => x.WeightKg)
                        .GreaterThan(0)
                        .When(x => x.WeightKg.HasValue);

                    item.RuleFor(x => x.UnitPrice)
                        .GreaterThanOrEqualTo(0);

                    item.RuleFor(x => x.SpecialNotes)
                        .MaximumLength(500);
                });
        }
    }
}
