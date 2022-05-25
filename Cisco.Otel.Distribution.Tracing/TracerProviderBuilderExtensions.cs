using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cisco.Otel.Distribution.Tracing;

public static class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddCiscoTracing(this TracerProviderBuilder builder)
    {
        var ciscoToken =
            Environment.GetEnvironmentVariable(Constants.CiscoTokenEnvVarName) ??
            throw new Exception("Could not find Cisco Token in environment variables");

        builder
            .SetResourceBuilder(
                ResourceBuilder
                    .CreateDefault()
                    .AddTelemetrySdk()
                    .AddCiscoVersion())
            .AddOtlpExporter(options => options.Headers = $"{Constants.TokenHeader}={ciscoToken}");

        builder.AddInstrumentation();

        return builder;
    }

    public static TracerProviderBuilder AddCiscoTracing(this TracerProviderBuilder builder, CiscoOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options),
                "No options have been set.");
        }

        builder
            .AddSource(options.ServiceName)
            .SetResourceBuilder(
                ResourceBuilder
                    .CreateEmpty()
                    .AddService(options.ServiceName)
                    .AddTelemetrySdk()
                    .AddCiscoVersion())
            .AddExporters(options.ExporterOptions, options.CiscoToken)
            .AddInstrumentation();

        return builder;
    }

    private static TracerProviderBuilder AddExporters(
        this TracerProviderBuilder builder,
        IEnumerable<ExporterOptions> exporterOptions,
        string ciscoToken)
    {
        foreach (var exporterOption in exporterOptions)
        {
            switch (exporterOption)
            {
                case ExporterOptions.Console:
                    builder.AddConsoleExporter();
                    break;
                case ExporterOptions.InMemory inMemory:
                    builder.AddInMemoryExporter(inMemory.ExportedItems);
                    break;
                case ExporterOptions.OtlpGrpc oltpGrpc:
                    builder.AddOtlpExporter(opts =>
                    {
                        if (oltpGrpc.CollectorEndpoint != null)
                        {
                            opts.Endpoint = new Uri(oltpGrpc.CollectorEndpoint);
                        }

                        opts.Protocol = OtlpExportProtocol.Grpc;
                        opts.Headers = $"{Constants.TokenHeader}={ciscoToken}";
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
                        opts.Headers = $"{Constants.TokenHeader}={ciscoToken}";
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(exporterOption));
            }
        }

        return builder;
    }

    private static TracerProviderBuilder AddInstrumentation(this TracerProviderBuilder builder)
    {
        builder.AddHttpClientInstrumentation();
        builder.AddSqlClientInstrumentation();

#if NET461
            builder.AddAspNetInstrumentation();
#endif

#if NETSTANDARD2_1
        // See https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Instrumentation.GrpcNetClient/README.md#suppressdownstreaminstrumentation
        builder.AddGrpcClientInstrumentation(options => options.SuppressDownstreamInstrumentation = true);
#endif

        return builder;
    }
}