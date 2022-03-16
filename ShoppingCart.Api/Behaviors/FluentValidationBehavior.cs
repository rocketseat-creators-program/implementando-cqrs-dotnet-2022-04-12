using FluentValidation;
using FluentValidation.Results;

using MediatR;

using ShoppingCart.Application;

namespace ShoppingCart.Api.Behaviors;

public class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> where TResponse : IResponse, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        List<ValidationResult>? failures = _validators
            .Select(v => v.Validate(request))
            .Where(f => f != null)
            .ToList();

        return failures.Any()
            ? Errors(failures)
            : next();
    }

    private static Task<TResponse> Errors(IEnumerable<ValidationResult> results)
    {
        IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        foreach (string? property in results.Where(x => !x.IsValid).SelectMany(x => x.Errors).Select(x => x.PropertyName).Distinct())
        {
            errors.Add(property, results
                .SelectMany(x => x.Errors)
                .Where(x => x.PropertyName == property)
                .Select(x => x.ErrorMessage)
                .ToArray());
        }

        TResponse response = new()
        {
            Success = false,
            Errors = errors
        };

        return Task.FromResult(response);
    }
}