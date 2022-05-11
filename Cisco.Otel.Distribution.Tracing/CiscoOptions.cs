namespace Cisco.Otel.Distribution.Tracing;

public class CiscoOptions
{
    public CiscoOptions(
        string ciscoToken,
        string? serviceName = null,
        ICollection<ExporterOptions>? exporterOptions = null)
    {
        CiscoToken =
            string.IsNullOrEmpty(ciscoToken)
                ? throw new ArgumentException("Cisco Token cannot be null or empty")
                : VerifyToken(ciscoToken);

        ServiceName =
            serviceName ?? Constants.DefaultServiceName;

        exporterOptions ??=
            new List<ExporterOptions>();

        if (!exporterOptions.Any())
            exporterOptions.Add(new ExporterOptions.OtlpGrpc());

        ExporterOptions = exporterOptions;
    }

    public string ServiceName { get; }
    public string CiscoToken { get; }
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