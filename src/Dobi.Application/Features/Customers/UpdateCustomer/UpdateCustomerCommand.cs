using Dobi.Contracts.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomer
{
    public sealed record UpdateCustomerCommand(
        int CustomerId,
        string FullName,
        string MobileNo,
        string Address,
        string? Email,
        bool IsActive) : IRequest<CustomerResponse>;
}
