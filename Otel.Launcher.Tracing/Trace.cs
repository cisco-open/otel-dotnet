using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Otel.Launcher.Tracing;

public static class Trace
{
    public static TracerProvider Init(CiscoOptions options)
    {
        return 
            Sdk.CreateTracerProviderBuilder()
                .AddCiscoTracing(options)
                .Build();
    }
}