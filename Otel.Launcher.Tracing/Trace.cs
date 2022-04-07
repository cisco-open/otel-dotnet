using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;

namespace Otel.Launcher.Tracing;

using OpenTelemetry;
using OpenTelemetry.Trace;

public static class Trace
{
    public static TracerProvider Init(Options options)
    {
        var tracerProviderBuilder =
            Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(options.ServiceName));

        foreach (var exporterOption in options.ExporterOptions)
        {
            switch (exporterOption)
            {
                case ExporterOptions.Console console:
                    tracerProviderBuilder.AddConsoleExporter();
                    break;
                case ExporterOptions.OltpGrpc oltpGrpc:
                    tracerProviderBuilder.AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = oltpGrpc.CollectorEndpoint;
                        opts.Protocol = OtlpExportProtocol.Grpc;
                        opts.Headers = $"{Constants.TokenHeader}={oltpGrpc.CiscoToken}";
                    });
                    break;
                case ExporterOptions.OltpHttp oltpHttp:
                    tracerProviderBuilder.AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = oltpHttp.CollectorEndpoint;
                        opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opts.Headers = $"{Constants.TokenHeader}={oltpHttp.CiscoToken}";
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(exporterOption));
            }
        }

        return tracerProviderBuilder.Build();
    }
}