namespace Otel.Launcher.Tracing;

public abstract class ExporterOptions
{
    private ExporterOptions()
    {
    }

    public class OltpHttp : ExporterOptions
    {
        public OltpHttp(Uri collectorEndpoint, string ciscoToken)
        {
            CollectorEndpoint = collectorEndpoint;
            CiscoToken = ciscoToken;
        }

        public Uri CollectorEndpoint { get; }
        public string CiscoToken { get; }
    }

    public class OltpGrpc : ExporterOptions
    {
        public OltpGrpc(Uri collectorEndpoint, string ciscoToken)
        {
            CollectorEndpoint = collectorEndpoint;
            CiscoToken = ciscoToken;
        }

        public Uri CollectorEndpoint { get; }
        public string CiscoToken { get; }
    }

    public class Console : ExporterOptions
    {
    }
}