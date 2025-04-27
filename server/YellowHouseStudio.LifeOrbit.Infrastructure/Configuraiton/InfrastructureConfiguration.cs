using Microsoft.Extensions.DependencyInjection;

namespace YellowHouseStudio.LifeOrbit.Infrastructure.Configuraiton;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddFamilyMemberConfiguration();
        return services;
    }
}
