using Microsoft.Extensions.Configuration;

namespace Cisco.Otel.Distribution.Tracing;

public static class CiscoOptionsHelper
{
    public static CiscoOptions FromConfiguration(IConfiguration configuration)
    {
        var ciscoOptionsFromConfig =
            configuration
                .GetSection(Constants.CiscoOptionsConfigName)
                .Get<CiscoOptionsFromConfig>();

        if (ciscoOptionsFromConfig == null)
            throw new Exception("Could not find Cisco Options in configuration file");

        var ciscoToken = ciscoOptionsFromConfig.CiscoToken ??
                         throw new Exception("Could not find Cisco Token in configuration file");

        var exporters =
            ciscoOptionsFromConfig.ExporterOptions?
                .Where(exporterOptions => exporterOptions.Type is not null)
                .Select(exporterOptions => GetExporterOptions(exporterOptions.Type, exporterOptions.Endpoint))
                .ToList();

        return
            new CiscoOptions(
                ciscoToken,
                ciscoOptionsFromConfig.ServiceName,
                ciscoOptionsFromConfig.Debug,
                ciscoOptionsFromConfig.PayloadsEnabled,
                ciscoOptionsFromConfig.MaxPayloadSize,
                exporters);
    }

    private static ExporterOptions GetExporterOptions(
        string? exporterType,
        string? collectorEndpoint = null)
    {
        return exporterType switch
        {
            Constants.GrpcExporterType => new ExporterOptions.OtlpGrpc(collectorEndpoint),
            Constants.HttpExporterType => new ExporterOptions.OtlpHttp(collectorEndpoint),
            Constants.ConsoleExporterType => new ExporterOptions.Console(),
            _ => throw new ArgumentException("Wrong exporter type", exporterType)
        };
    }
}

internal class CiscoOptionsFromConfig
{
    public string? ServiceName { get; set; }
    public string? CiscoToken { get; set; }
    public bool? Debug { get; set; }
    public bool? PayloadsEnabled { get; set; }
    public int? MaxPayloadSize { get; set; }
    public ExporterOptionsFromConfig[]? ExporterOptions { get; set; }
}

internal class ExporterOptionsFromConfig
{
    public string? Type { get; set; }
    public string? Endpoint { get; set; }
}