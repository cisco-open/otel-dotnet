namespace Cisco.Otel.Distribution.Tracing;

public static class Constants
{
    public const string TokenHeader = "x-epsagon-token";

    public const string HttpExporterType = "otlp-http";
    public const string GrpcExporterType = "otlp-grpc";
    public const string ConsoleExporterType = "console";

    public const string DefaultServiceName = "application";
    public const string DefaultExporterType = GrpcExporterType;

    public const string CiscoOptionsConfigName = "CiscoOptions";

    public const string CiscoTokenEnvVarName = "CISCO_TOKEN";

    // public const string ServiceNameEnvironmentVariableName = "OTEL_SERVICE_NAME";
    // public const string CollectorEndpointEnvironmentVariableName = "OTEL_COLLECTOR_ENDPOINT";
    // public const string ExporterTypeEnvironmentVariableName = "OTEL_EXPORTER_TYPE";
}