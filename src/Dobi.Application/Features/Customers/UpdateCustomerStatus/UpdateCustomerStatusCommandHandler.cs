using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Customers;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomerStatus
{
    public sealed class UpdateCustomerStatusCommandHandler
    : IRequestHandler<UpdateCustomerStatusCommand, CustomerResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateCustomerStatusCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CustomerResponse> Handle(
            UpdateCustomerStatusCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .Include(x => x.CustomerType)
                .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException("Customer", request.CustomerId);
            }

            customer.IsActive = request.IsActive;
            customer.UpdatedAt = _dateTimeProvider.UtcNow;
            customer.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CustomerResponse(
                customer.Id,
                customer.CustomerNo,
                customer.CustomerTypeId,
                customer.CustomerType.CustomerTypeName,
                customer.FullName,
                customer.MobileNo,
                customer.Address,
                customer.Email,
                customer.IsActive,
                customer.CreatedAt);
        }
    }
}
