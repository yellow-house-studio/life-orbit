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

        if (context.Exception is YellowHouseStudio.LifeOrbit.Application.Common.Exceptions.NotFoundException notFoundException)
        {
            var notFoundDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = notFoundException.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };
            context.Result = new NotFoundObjectResult(notFoundDetails);
            context.ExceptionHandled = true;
            return;
        }

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