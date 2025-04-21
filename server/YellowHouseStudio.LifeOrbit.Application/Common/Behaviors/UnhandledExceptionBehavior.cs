using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public UnhandledExceptionBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, 
                "Unhandled Exception for Request {RequestName} {@Request}", 
                requestName, request);

            throw;
        }
    }
} 