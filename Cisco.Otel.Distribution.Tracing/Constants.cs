namespace Cisco.Otel.Distribution.Tracing;

public static class Constants
{
    public const string TokenHeader = "authorization";

    public const string HttpExporterType = "otlp-http";
    public const string GrpcExporterType = "otlp-grpc";
    public const string ConsoleExporterType = "console";

    public const string DefaultServiceName = "application";
    public const string DefaultCollectorEndpoint = "https://production.cisco-udp.com/trace-collector";

    public const string CiscoOptionsConfigName = "CiscoOptions";

    public const string CiscoTokenEnvVarName = "CISCO_TOKEN";

    public const string DefaultSdkVersion = "version not supported";
}
