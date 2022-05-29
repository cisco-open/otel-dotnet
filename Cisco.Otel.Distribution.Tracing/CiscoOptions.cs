using Cisco.Opentelemetry.Specifications.Consts;

namespace Cisco.Otel.Distribution.Tracing;

public class CiscoOptions
{
    public CiscoOptions(
        string ciscoToken,
        string? serviceName = null,
        bool? debug = null,
        bool? payloadsEnabled = null,
        int? maxPayloadSize = null,
        ICollection<ExporterOptions>? exporterOptions = null)
    {
        CiscoToken =
            string.IsNullOrEmpty(ciscoToken)
                ? throw new ArgumentException("Cisco Token cannot be null or empty")
                : VerifyToken(ciscoToken);

        ServiceName = serviceName ??
            Constants.DefaultServiceName;

        Debug = debug ??
            Utils.ReadFromEnv(Consts.CISCO_DEBUG_ENV, Consts.DEFAULT_CISCO_DEBUG);

        MaxPayloadSize = maxPayloadSize ??
            Utils.ReadFromEnv(Consts.CISCO_MAX_PAYLOAD_SIZE_ENV, Consts.DEFAULT_MAX_PAYLOAD_SIZE);

        PayloadsEnabled = payloadsEnabled ??
            Utils.ReadFromEnv(Consts.CISCO_PAYLOADS_ENABLED_ENV, Consts.DEFAULT_PAYLOADS_ENABLED);


        exporterOptions ??=
            new List<ExporterOptions>();

        if (!exporterOptions.Any())
            exporterOptions.Add(new ExporterOptions.OtlpHttp(Constants.DefaultCollectorEndpoint));

        ExporterOptions = exporterOptions;
    }

    public string ServiceName { get; }
    public string CiscoToken { get; }
    public bool Debug { get; }
    public bool PayloadsEnabled { get; }
    public int MaxPayloadSize { get; }
    public IEnumerable<ExporterOptions> ExporterOptions { get; }

    private static string VerifyToken(string token)
    {
        const string authenticationPrefix = "Bearer";

        return
            token.StartsWith(authenticationPrefix)
                ? token
                : string.Join(" ", authenticationPrefix, token);
    }
}