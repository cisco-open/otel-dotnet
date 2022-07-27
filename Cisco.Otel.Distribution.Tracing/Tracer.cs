using OpenTelemetry;
using OpenTelemetry.Trace;
using Cisco.Opentelemetry.Specifications.Consts;

namespace Cisco.Otel.Distribution.Tracing;

public static class Tracer
{
    public static TracerProvider Init()
    {
        var tracerProvider =
            Sdk.CreateTracerProviderBuilder()
                .AddCiscoTracing()
                .Build();

        Utils.PrintTelescopeIsRunning();
        return tracerProvider;
    }

    public static TracerProvider Init(CiscoOptions options)
    {
        var tracerProvider =
            Sdk.CreateTracerProviderBuilder()
            .AddCiscoTracing(options)
            .Build();

        Utils.PrintTelescopeIsRunning();
        return tracerProvider;
    }
}