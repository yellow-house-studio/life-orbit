using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace YellowHouseStudio.LifeOrbit.Api.Infrastructure.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly Serilog.ILogger _logger;
    private readonly bool _isDevelopment;

    public GlobalExceptionFilter(Serilog.ILogger logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _isDevelopment = env.IsDevelopment();
    }

    public void OnException(ExceptionContext context)
    {
        Console.WriteLine("GlobalExceptionFilter");
        _logger.Error(context.Exception, 
            "Unhandled exception occurred while executing {Path}", 
            context.HttpContext.Request.Path);

        var details = _isDevelopment
            ? new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Detail = context.Exception.ToString(),
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
            : new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
} 