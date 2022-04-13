namespace Cisco.Otel.Distribution.Tracing;

public class CiscoOptions
{
    public CiscoOptions(
        ICollection<ExporterOptions> exporterOptions, 
        string? serviceName = null)
    {
        if (!exporterOptions.Any())
            throw new ArgumentException("Must contain one or more exporter options");

        ExporterOptions = exporterOptions;
        ServiceName = serviceName ?? Constants.DefaultServiceName;
    }

    public string ServiceName { get; }
    public IEnumerable<ExporterOptions> ExporterOptions { get; }
}