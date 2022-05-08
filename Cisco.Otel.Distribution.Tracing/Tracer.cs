using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Cisco.Otel.Distribution.Tracing;

public static class Tracer
{
    public static TracerProvider Init()
    {
        return
            Sdk.CreateTracerProviderBuilder()
                .AddCiscoTracing()
                .Build();
    }
    
    public static TracerProvider Init(CiscoOptions options)
    {
        return
            Sdk.CreateTracerProviderBuilder()
                .AddCiscoTracing(options)
                .Build();
    }
}