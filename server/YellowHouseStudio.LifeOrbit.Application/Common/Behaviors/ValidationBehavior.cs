using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Commands;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

/// <summary>
/// Enforces validation for commands that have inputs.
/// A command is considered to have inputs if it has any public properties or constructor parameters.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only validate if it's a command
        if (request is not ICommand<TResponse> and not ICommand)
        {
            return await next();
        }

        // Check if the command has any inputs (public properties or constructor parameters)
        var hasInputs = HasInputs(request);

        if (hasInputs && !_validators.Any())
        {
            var error = $"Command {typeof(TRequest).Name} has inputs but no validator is registered. All commands with inputs must have at least one validator.";
            _logger.LogError(error);
            throw new InvalidOperationException(error);
        }

        if (_validators.Any())
        {
            _logger.LogDebug("Validating command {CommandType}", typeof(TRequest).Name);
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                _logger.LogWarning("Validation failed for command {CommandType} with {ErrorCount} errors", 
                    typeof(TRequest).Name, failures.Count);
                throw new ValidationException(failures);
            }

            _logger.LogDebug("Validation successful for command {CommandType}", typeof(TRequest).Name);
        }

        return await next();
    }

    private static bool HasInputs(TRequest request)
    {
        var type = request.GetType();

        // Check for public properties
        if (type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Any())
        {
            return true;
        }

        // Check constructor parameters
        var constructors = type.GetConstructors();
        return constructors.Any(c => c.GetParameters().Length > 0);
    }
} 