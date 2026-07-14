using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(validator =>
                    validator.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .GroupBy(
                    failure => failure.PropertyName,
                    failure => failure.ErrorMessage)
                .ToDictionary(
                    group => group.Key,
                    group => group.Distinct().ToArray());

            if (errors.Count != 0)
            {
                throw new Dobi.Shared.Exceptions.ValidationException(errors);
            }

            return await next();
        }
    }
}
