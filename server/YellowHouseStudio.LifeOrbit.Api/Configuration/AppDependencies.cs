using YellowHouseStudio.LifeOrbit.Api.Services;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Api.Configuration;

public static class AppDependencies
{
    public static IServiceCollection AddAppDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, FakeCurrentUser>();
        return services;
    }
}