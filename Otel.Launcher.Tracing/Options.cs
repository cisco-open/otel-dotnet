namespace Otel.Launcher.Tracing;

public class Options
{
    public Options(string serviceName, IEnumerable<ExporterOptions> exporterOptions)
    {
        ServiceName = serviceName;
        ExporterOptions = exporterOptions;
    }

    public string ServiceName { get; }
    public IEnumerable<ExporterOptions> ExporterOptions { get; }
}