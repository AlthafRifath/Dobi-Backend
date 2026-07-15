using Dobi.Contracts.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
        string FullName,
        string MobileNo,
        string Address,
        string? Email,
        int CustomerTypeId) : IRequest<CustomerResponse>;
}
