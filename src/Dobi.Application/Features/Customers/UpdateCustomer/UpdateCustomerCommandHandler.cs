using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Customers;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomer
{
    public sealed class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateCustomerCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CustomerResponse> Handle(
            UpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .Include(x => x.CustomerType)
                .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException("Customer", request.CustomerId);
            }

            var fullName = request.FullName.Trim();
            var mobileNo = request.MobileNo.Trim();
            var address = request.Address.Trim();
            var email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

            var duplicateMobileExists = await _dbContext.Customers.AnyAsync(
                x => x.Id != request.CustomerId &&
                     x.MobileNo == mobileNo,
                cancellationToken);

            if (duplicateMobileExists)
            {
                throw new ConflictException("A customer with the same mobile number already exists.");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var duplicateEmailExists = await _dbContext.Customers.AnyAsync(
                    x => x.Id != request.CustomerId &&
                         x.Email != null &&
                         x.Email.ToLower() == email.ToLower(),
                    cancellationToken);

                if (duplicateEmailExists)
                {
                    throw new ConflictException("A customer with the same email already exists.");
                }
            }

            customer.FullName = fullName;
            customer.MobileNo = mobileNo;
            customer.Address = address;
            customer.Email = email;
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
