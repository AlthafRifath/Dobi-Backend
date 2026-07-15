using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.GetCustomers
{
    public sealed class GetCustomersQueryHandler
    : IRequestHandler<GetCustomersQuery, PagedResponse<CustomerResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetCustomersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<CustomerResponse>> Handle(
            GetCustomersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Customers
                .AsNoTracking()
                .Include(x => x.CustomerType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.CustomerNo.ToLower().Contains(searchTerm) ||
                    x.FullName.ToLower().Contains(searchTerm) ||
                    x.MobileNo.ToLower().Contains(searchTerm) ||
                    x.Address.ToLower().Contains(searchTerm) ||
                    (x.Email != null && x.Email.ToLower().Contains(searchTerm)));
            }

            if (request.CustomerTypeId.HasValue)
            {
                query = query.Where(x => x.CustomerTypeId == request.CustomerTypeId.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var customers = await query
                .OrderBy(x => x.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<CustomerResponse>(
                customers,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
