using System.Diagnostics;

namespace Cisco.Otel.Distribution.Tracing;

public abstract class ExporterOptions
{
    private ExporterOptions()
    {
    }

    public class OtlpHttp : ExporterOptions
    {
        public OtlpHttp(string? collectorEndpoint = null)
        {
            CollectorEndpoint = collectorEndpoint;
        }

        public string? CollectorEndpoint { get; }
    }

    public class OtlpGrpc : ExporterOptions
    {
        public OtlpGrpc(string? collectorEndpoint = null)
        {
            CollectorEndpoint = collectorEndpoint;
        }
        
        public string? CollectorEndpoint { get; }
    }

    public class InMemory : ExporterOptions
    {
        public InMemory(ICollection<Activity> exportedItems)
        {
            ExportedItems = exportedItems;
        }
        
        public ICollection<Activity> ExportedItems { get; }
    }
    
    public class Console : ExporterOptions
    {
    }
}