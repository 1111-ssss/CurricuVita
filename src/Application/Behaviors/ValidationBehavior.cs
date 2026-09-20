using System.Net;
using Domain.ResultPattern.Errors;
using Domain.ResultPattern.Result;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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
        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        return CreateFailure(failures);
    }

    private static TResponse CreateFailure(List<ValidationFailure> failures)
    {
        var first = failures[0];
        var code = string.IsNullOrWhiteSpace(first.ErrorCode)
            ? Errors.ValidationFailed.Code
            : first.ErrorCode;
        var message = string.IsNullOrWhiteSpace(first.ErrorMessage)
            ? Errors.ValidationFailed.Message
            : first.ErrorMessage;
        var details = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(g => g.Key, g => g.First().ErrorMessage);
        var error = new Error(HttpStatusCode.BadRequest, code, message);

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)new Result(error, details);
        }

        if (typeof(TResponse).IsGenericType
            && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var inner = typeof(TResponse).GetGenericArguments()[0];
            var defaultValue = inner.IsValueType ? Activator.CreateInstance(inner) : null;
            return (TResponse)Activator.CreateInstance(typeof(TResponse), defaultValue, error, details)!;
        }

        throw new InvalidOperationException(
            $"ValidationBehavior supports only {nameof(Result)} responses, got {typeof(TResponse).Name}.");
    }
}
