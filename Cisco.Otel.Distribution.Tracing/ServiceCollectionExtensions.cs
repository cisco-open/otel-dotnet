using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Cisco.Otel.Distribution.Tracing;

public static class ServiceCollectionExtensions
{
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