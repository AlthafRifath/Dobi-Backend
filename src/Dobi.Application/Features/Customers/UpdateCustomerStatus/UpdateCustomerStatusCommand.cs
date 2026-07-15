using Dobi.Contracts.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomerStatus
{
    public sealed record UpdateCustomerStatusCommand(
        int CustomerId,
        bool IsActive) : IRequest<CustomerResponse>;
}
