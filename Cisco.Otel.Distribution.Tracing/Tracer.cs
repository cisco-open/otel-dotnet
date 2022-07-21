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

        PrintTelescopeIsRunning();
        return tracerProvider;
    }

    public static TracerProvider Init(CiscoOptions options)
    {
        var tracerProvider =
            Sdk.CreateTracerProviderBuilder()
            .AddCiscoTracing(options)
            .Build();

        PrintTelescopeIsRunning();
        return tracerProvider;
    }

    private static void PrintTelescopeIsRunning()
    {
        Console.WriteLine(Consts.TELESCOPE_IS_RUNNING_MESSAGE);
    }
}