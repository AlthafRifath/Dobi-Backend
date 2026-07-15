using Dobi.Contracts.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.GetCustomerById
{
    public sealed record GetCustomerByIdQuery(
        int CustomerId) : IRequest<CustomerResponse>;
}
