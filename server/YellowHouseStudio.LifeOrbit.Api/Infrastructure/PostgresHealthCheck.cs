using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace YellowHouseStudio.LifeOrbit.Api.Infrastructure;

public class PostgresHealthCheck : IHealthCheck
{
    private readonly NpgsqlDataSource _dataSource;
    public PostgresHealthCheck(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var conn = _dataSource.CreateConnection();
            await conn.OpenAsync(cancellationToken);
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            await cmd.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
} 