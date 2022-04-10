using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Otel.Launcher.Tracing;

public static class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddCiscoTracing(this TracerProviderBuilder builder, CiscoOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options),
                "No options have been set in appsettings.json or environment variables.");
        }

        builder
            .AddSource(options.ServiceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(options.ServiceName));

        foreach (var exporterOption in options.ExporterOptions)
        {
            switch (exporterOption)
            {
                case ExporterOptions.Console:
                    builder.AddConsoleExporter();
                    break;
                case ExporterOptions.OtlpGrpc oltpGrpc:
                    builder.AddOtlpExporter(opts =>
                    {
                        if (oltpGrpc.CollectorEndpoint != null)
                        {
                            opts.Endpoint = new Uri(oltpGrpc.CollectorEndpoint);
                        }

                        opts.Protocol = OtlpExportProtocol.Grpc;
                        opts.Headers = $"{Constants.TokenHeader}={oltpGrpc.CiscoToken}";
                    });
                    break;
                case ExporterOptions.OtlpHttp oltpHttp:
                    builder.AddOtlpExporter(opts =>
                    {
                        if (oltpHttp.CollectorEndpoint != null)
                        {
                            opts.Endpoint = new Uri(oltpHttp.CollectorEndpoint);
                        }

                        opts.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opts.Headers = $"{Constants.TokenHeader}={oltpHttp.CiscoToken}";
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(exporterOption));
            }
        }

        if (options.InstrumentHttpClient)
        {
            builder.AddHttpClientInstrumentation();
        }

        if (options.InstrumentSqlClient)
        {
            builder.AddSqlClientInstrumentation();
        }

#if NET461
            builder.AddAspNetInstrumentation();
#endif

#if NETSTANDARD2_1
            if (options.InstrumentGrpcClient && options.InstrumentHttpClient) // HttpClient needs to be instrumented for GrpcClient instrumentation to work.
            {
                // See https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Instrumentation.GrpcNetClient/README.md#suppressdownstreaminstrumentation
                builder.AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true);
            }
#endif

        return builder;
    }
}