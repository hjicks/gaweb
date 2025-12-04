using FluentValidation;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Pipelines
{
    public sealed class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, Result>
        where TRequest : IRequest<Result>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<Result> Handle(
            TRequest request,
            RequestHandlerDelegate<Result> next,
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
                return Result.Failure(StatusCodes.Status409Conflict, errors);
            }

            return await next();
        }
    }
}
