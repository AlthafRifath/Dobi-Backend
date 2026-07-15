using Dobi.Contracts.Common;
using Dobi.Contracts.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.GetCustomers
{
    public sealed record GetCustomersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? CustomerTypeId,
        bool? IsActive) : IRequest<PagedResponse<CustomerResponse>>;
}
