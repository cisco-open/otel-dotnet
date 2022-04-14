using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Cisco.Otel.Distribution.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCiscoTracing(this IServiceCollection services)
    {
        return services.AddCiscoTracing(CiscoOptionsHelper.FromEnvironmentVariables());
    }

    public static IServiceCollection AddCiscoTracing(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCiscoTracing(CiscoOptionsHelper.FromConfiguration(configuration));
    }

    public static IServiceCollection AddCiscoTracing(this IServiceCollection services, CiscoOptions options)
    {
#if (NETSTANDARD2_0_OR_GREATER)
        services
            .AddOpenTelemetryTracing(hostingBuilder => hostingBuilder
                .Configure((serviceProvider, builder) =>
                {
                    builder
                        .AddCiscoTracing(options)
                        .AddAspNetCoreInstrumentation(opts => { opts.RecordException = true; });
                })
            )
            .AddSingleton(TracerProvider.Default.GetTracer(options.ServiceName));
#endif
        return services;
    }
}