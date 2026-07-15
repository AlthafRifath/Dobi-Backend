using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Customers;
using Dobi.Domain.Customers;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.CreateCustomer
{
    public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateCustomerCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CustomerResponse> Handle(
            CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var fullName = request.FullName.Trim();
            var mobileNo = request.MobileNo.Trim();
            var address = request.Address.Trim();
            var email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

            var customerType = await _dbContext.CustomerTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.CustomerTypeId, cancellationToken);

            if (customerType is null)
            {
                throw new NotFoundException("Customer type", request.CustomerTypeId);
            }

            var mobileExists = await _dbContext.Customers.AnyAsync(
                x => x.MobileNo == mobileNo,
                cancellationToken);

            if (mobileExists)
            {
                throw new ConflictException("A customer with the same mobile number already exists.");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailExists = await _dbContext.Customers.AnyAsync(
                    x => x.Email != null && x.Email.ToLower() == email.ToLower(),
                    cancellationToken);

                if (emailExists)
                {
                    throw new ConflictException("A customer with the same email already exists.");
                }
            }

            var temporaryCustomerNo = $"TMP-{Guid.NewGuid():N}"[..30];

            var customer = new Customer
            {
                CustomerNo = temporaryCustomerNo,
                CustomerTypeId = request.CustomerTypeId,
                FullName = fullName,
                MobileNo = mobileNo,
                Address = address,
                Email = email,
                IsActive = true,
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Customers.Add(customer);

            await _dbContext.SaveChangesAsync(cancellationToken);

            customer.CustomerNo = $"CUST-{customer.Id:D6}";

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CustomerResponse(
                customer.Id,
                customer.CustomerNo,
                customer.CustomerTypeId,
                customerType.CustomerTypeName,
                customer.FullName,
                customer.MobileNo,
                customer.Address,
                customer.Email,
                customer.IsActive,
                customer.CreatedAt);
        }
    }
}
