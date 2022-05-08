using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Cisco.Otel.Distribution.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCiscoTracing(this IServiceCollection services)
    {
        return services.AddCiscoTracing(builder => builder.AddCiscoTracing());
    }

    public static IServiceCollection AddCiscoTracing(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddCiscoTracing(CiscoOptionsHelper.FromConfiguration(configuration));
    }

    public static IServiceCollection AddCiscoTracing(
        this IServiceCollection services,
        CiscoOptions options)
    {
        return services.AddCiscoTracing(builder => builder.AddCiscoTracing(options));
    }

    private static IServiceCollection AddCiscoTracing(
        this IServiceCollection services,
        Action<TracerProviderBuilder> action)
    {
#if (NETSTANDARD2_0_OR_GREATER)
        services
            .AddOpenTelemetryTracing(builder =>
            {
                action.Invoke(builder);
                builder.AddAspNetCoreInstrumentation(opts => opts.RecordException = true);
            });
#endif
        return services;
    }
}