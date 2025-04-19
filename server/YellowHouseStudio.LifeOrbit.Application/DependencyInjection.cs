using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

namespace YellowHouseStudio.LifeOrbit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            
            // Add behaviors in the order they should be executed
            // 1. Logging (outermost - logs everything including other behaviors)
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

             // 3. Exception handling (catches exceptions from transaction and handler)
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            
            // 2. Validation (validates before any other processing)
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
           
            // 4. Transaction (innermost - wraps the actual handler)
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

        // Register all validators from assembly
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
} 