using MediatR;
using Microsoft.Extensions.Logging;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid().ToString();

        _logger.LogInformation("Beginning request {RequestName} [{RequestId}]", requestName, requestId);
        
        try
        {
            var response = await next();

            _logger.LogInformation("Completed request {RequestName} [{RequestId}]", 
                requestName, requestId);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {RequestName} [{RequestId}] failed", 
                requestName, requestId);
            throw;
        }
    }
} 