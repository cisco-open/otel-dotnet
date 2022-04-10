namespace Otel.Launcher.Tracing;

public abstract class ExporterOptions
{
    private ExporterOptions()
    {
    }

    public class OltpHttp : ExporterOptions
    {
        public OltpHttp(string ciscoToken, string? collectorEndpoint = null)
        {
            CiscoToken = ciscoToken;
            CollectorEndpoint = collectorEndpoint;
        }
        
        public string CiscoToken { get; }
        public string? CollectorEndpoint { get; }
    }

    public class OltpGrpc : ExporterOptions
    {
        public OltpGrpc(string ciscoToken, string? collectorEndpoint = null)
        {
            CiscoToken = ciscoToken;
            CollectorEndpoint = collectorEndpoint;
        }
        
        public string CiscoToken { get; }
        public string? CollectorEndpoint { get; }
    }

    public class Console : ExporterOptions
    {
    }
}