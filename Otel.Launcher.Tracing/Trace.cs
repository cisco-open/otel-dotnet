namespace Otel.Launcher.Tracing;

using OpenTelemetry;
using OpenTelemetry.Trace;

public static class Trace
{
    public static void Init()
    {
        Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter()
            .AddConsoleExporter()
            .Build();
    }
}   