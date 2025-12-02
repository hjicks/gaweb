using FluentValidation;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Pipelines
{
    public sealed class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
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
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context)));

            string errors = string.Join(Environment.NewLine,
                validationResults
                    .SelectMany(validationResult => validationResult.Errors)
                    .Where(validationFailure => validationFailure != null)
                    .Select(failure => failure.ErrorMessage));

            if (errors.Any())
            {
                object result = Result.Failure(StatusCodes.Status409Conflict, errors);
                return (TResponse)result;
            }

            return await next();
        }
    }
}
