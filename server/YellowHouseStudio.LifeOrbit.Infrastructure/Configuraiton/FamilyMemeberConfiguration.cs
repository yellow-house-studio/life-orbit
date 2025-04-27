using Microsoft.Extensions.DependencyInjection;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;

namespace YellowHouseStudio.LifeOrbit.Infrastructure.Configuraiton;

internal static class FamilyMemberConfiguration
{
    public static IServiceCollection AddFamilyMemberConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IFamilyMemberRepository, FamilyMemberRepository>();
        return services;
    }
}
