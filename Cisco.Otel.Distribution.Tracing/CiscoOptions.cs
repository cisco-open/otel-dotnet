namespace Cisco.Otel.Distribution.Tracing;

public class CiscoOptions
{
    public CiscoOptions(
        ICollection<ExporterOptions> exporterOptions, 
        string serviceName = Constants.DefaultServiceName,
        bool instrumentHttpClient = true,
        bool instrumentSqlClient = true,
        bool instrumentGrpcClient = true)
    {
        if (!exporterOptions.Any())
            throw new ArgumentException("Must contain one or more exporter options");
        
        ExporterOptions = exporterOptions;
        ServiceName = serviceName;
        InstrumentHttpClient = instrumentHttpClient;
        InstrumentSqlClient = instrumentSqlClient;
        InstrumentGrpcClient = instrumentGrpcClient;
    }

    public string ServiceName { get; }
    public IEnumerable<ExporterOptions> ExporterOptions { get; }
    public bool InstrumentHttpClient { get; }
    public bool InstrumentSqlClient { get; }
    public bool InstrumentGrpcClient { get; }
}