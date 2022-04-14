using Microsoft.Extensions.Configuration;

namespace Cisco.Otel.Distribution.Tracing;

public static class CiscoOptionsHelper
{
    public static CiscoOptions FromEnvironmentVariables()
    {
        var serviceName = Environment.GetEnvironmentVariable(Constants.ServiceNameEnvironmentVariableName);

        var ciscoToken =
            Environment.GetEnvironmentVariable(Constants.CiscoTokenEnvironmentVariableName) ??
            throw new Exception("Could not find Cisco Token in environment variables");

        var exporterType = 
            Environment.GetEnvironmentVariable(Constants.ExporterTypeEnvironmentVariableName);

        var collectorEndpoint = 
            Environment.GetEnvironmentVariable(Constants.CollectorEndpointEnvironmentVariableName);

        if (exporterType is not null)
        {
            return
                new CiscoOptions(
                    ciscoToken,
                    serviceName,
                    new List<ExporterOptions> {GetExporterOptions(exporterType, collectorEndpoint)});
        }

        return
            new CiscoOptions(
                ciscoToken,
                serviceName);
    }

    public static CiscoOptions FromConfiguration(IConfiguration configuration)
    {
        var ciscoOptionsFromConfig =
            configuration
                .GetSection(Constants.ConfigurationSectionName)
                .Get<CiscoOptionsFromConfig>();

        if (ciscoOptionsFromConfig == null)
        {
            throw new Exception("Could not find Cisco Options in configuration file");
        }

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
                exporters);
    }

    private static ExporterOptions GetExporterOptions(
        string exporterType,
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
    public string CiscoToken { get; set; }
    public ExporterOptionsFromConfig[]? ExporterOptions { get; set; }
}

internal class ExporterOptionsFromConfig
{
    public string? Type { get; set; }
    public string? Endpoint { get; set; }
}