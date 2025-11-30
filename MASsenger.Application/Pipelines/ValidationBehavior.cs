using FluentValidation;
using MediatR;
using System.Reflection;

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

            string description = string.Join(Environment.NewLine,
                validationResults
                    .SelectMany(validationResult => validationResult.Errors)
                    .Where(validationFailure => validationFailure != null)
                    .Select(failure => failure.ErrorMessage));

            if (description.Any())
            {
                var resultType = typeof(TResponse);
                var result = Activator.CreateInstance(resultType);
                foreach (PropertyInfo property in resultType.GetProperties())
                {
                    if (property.Name == "Success") property.SetValue(result, false, null);
                    if (property.Name == "StatusCode") property.SetValue(result, System.Net.HttpStatusCode.Conflict, null);
                    if (property.Name == "Description") property.SetValue(result, description, null);
                }
                return (TResponse)result!;
            }

            return await next();
        }
    }
}
