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

        var exporterType = Environment.GetEnvironmentVariable(Constants.ExporterTypeEnvironmentVariableName) ??
                           Constants.DefaultExporterType;

        var collectorEndpoint = Environment.GetEnvironmentVariable(Constants.CollectorEndpointEnvironmentVariableName);

        var exporterOptions = GetExporterOptions(exporterType, ciscoToken, collectorEndpoint);

        return
            new CiscoOptions(
                new List<ExporterOptions> {exporterOptions},
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

        var exporterType = ciscoOptionsFromConfig.ExporterOptions?.Type ?? Constants.DefaultExporterType;

        var collectorEndpoint = ciscoOptionsFromConfig.ExporterOptions?.Endpoint;

        var exporterOptions = GetExporterOptions(exporterType, ciscoToken, collectorEndpoint);

        return
            new CiscoOptions(
                new List<ExporterOptions> {exporterOptions},
                ciscoOptionsFromConfig.ServiceName);
    }

    private static ExporterOptions GetExporterOptions(
        string exporterType,
        string ciscoToken,
        string? collectorEndpoint = null)
    {
        return exporterType switch
        {
            Constants.GrpcExporterType => new ExporterOptions.OtlpGrpc(ciscoToken, collectorEndpoint),
            Constants.HttpExporterType => new ExporterOptions.OtlpHttp(ciscoToken, collectorEndpoint),
            Constants.ConsoleExporterType => new ExporterOptions.Console(),
            _ => throw new ArgumentException("Wrong exporter type", exporterType)
        };
    }
}

internal class CiscoOptionsFromConfig
{
    public string ServiceName { get; set; }
    public string CiscoToken { get; set; }
    public ExporterOptionsFromConfig ExporterOptions { get; set; }
}

internal class ExporterOptionsFromConfig
{
    public string Type { get; set; }
    public string Endpoint { get; set; }
}