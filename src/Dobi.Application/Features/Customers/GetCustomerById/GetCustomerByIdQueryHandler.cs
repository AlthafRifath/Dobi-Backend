using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Customers;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.GetCustomerById
{
    public sealed class GetCustomerByIdQueryHandler
    : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetCustomerByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerResponse> Handle(
            GetCustomerByIdQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .Include(x => x.CustomerType)
                .Where(x => x.Id == request.CustomerId)
                .Select(x => new CustomerResponse(
                    x.Id,
                    x.CustomerNo,
                    x.CustomerTypeId,
                    x.CustomerType.CustomerTypeName,
                    x.FullName,
                    x.MobileNo,
                    x.Address,
                    x.Email,
                    x.IsActive,
                    x.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException("Customer", request.CustomerId);
            }

            return customer;
        }
    }
}
