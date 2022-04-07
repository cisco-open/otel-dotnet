using OpenTelemetry.Exporter;

namespace Otel.Launcher.Tracing;

using OpenTelemetry;
using OpenTelemetry.Trace;

public static class Trace
{
    public static TracerProvider Init()
    {
        return 
            Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(opts =>
                {
                    opts.Endpoint = new Uri("http://localhost:4317");
                    opts.Protocol = OtlpExportProtocol.Grpc;
                })
                .AddConsoleExporter()
                .Build();
    }
}   