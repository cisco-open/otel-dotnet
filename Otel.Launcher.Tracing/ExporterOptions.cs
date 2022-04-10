namespace Otel.Launcher.Tracing;

public abstract class ExporterOptions
{
    private ExporterOptions()
    {
    }

    public class OtlpHttp : ExporterOptions
    {
        public OtlpHttp(string ciscoToken, string? collectorEndpoint = null)
        {
            CiscoToken = ciscoToken ??
                         throw new ArgumentException("Cisco Token cannot be null");
            CollectorEndpoint = collectorEndpoint;
        }

        public string CiscoToken { get; }
        public string? CollectorEndpoint { get; }
    }

    public class OtlpGrpc : ExporterOptions
    {
        public OtlpGrpc(string ciscoToken, string? collectorEndpoint = null)
        {
            CiscoToken = ciscoToken ??
                         throw new ArgumentException("Cisco Token cannot be null");
            CollectorEndpoint = collectorEndpoint;
        }

        public string CiscoToken { get; }
        public string? CollectorEndpoint { get; }
    }

    public class Console : ExporterOptions
    {
    }
}