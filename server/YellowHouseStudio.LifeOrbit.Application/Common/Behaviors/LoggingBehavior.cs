using MediatR;
using Microsoft.Extensions.Logging;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        _logger.LogInformation(
            "Handling {RequestName} {@Request}",
            requestName, request);

        var response = await next();

        _logger.LogInformation(
            "Handled {RequestName} with {@Response}",
            requestName, response);

        return response;
    }
} 