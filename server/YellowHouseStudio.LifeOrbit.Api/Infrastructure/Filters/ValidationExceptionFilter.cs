using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YellowHouseStudio.LifeOrbit.Api.Infrastructure.Filters;

public class ValidationExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        Console.WriteLine("ValidationExceptionFilter");
        if (context.Exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray()
                );

            var validationProblemDetails = new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details.",
                Instance = context.HttpContext.Request.Path
            };

            if(validationException.Errors.Any(e => e.ErrorCode == ValidationErrorCodes.NotFound))
            {
                context.Result = new NotFoundObjectResult(validationProblemDetails);
            } else {
                 context.Result = new BadRequestObjectResult(validationProblemDetails);
            }
            
            context.ExceptionHandled = true;
        }
    }
} 