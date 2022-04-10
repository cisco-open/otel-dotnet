namespace Otel.Launcher.Tracing;

public class CiscoOptions
{
    public CiscoOptions(
        ICollection<ExporterOptions> exporterOptions, 
        string serviceName = "Application",
        bool instrumentHttpClient = true,
        bool instrumentSqlClient = true,
        bool instrumentGrpcClient = true)
    {
        if (!exporterOptions.Any())
            throw new ArgumentException("exporterOptions must contain one or more exporter options");
        
        ExporterOptions = exporterOptions;
        ServiceName = serviceName;
        InstrumentHttpClient = instrumentHttpClient;
        InstrumentSqlClient = instrumentSqlClient;
        InstrumentGrpcClient = instrumentGrpcClient;
    }

    public string ServiceName { get; }
    public IEnumerable<ExporterOptions> ExporterOptions { get; }

    /// <summary>
    /// Controls whether to instrument HttpClient calls.
    /// </summary>
    public bool InstrumentHttpClient { get; }

    /// <summary>
    /// Controls whether to instrument SqlClient calls.
    /// </summary>
    public bool InstrumentSqlClient { get; }

    /// <summary>
    /// Controls whether to instrument GrpcClient calls when running on .NET Standard 2.1 or greater.
    /// Requires <see cref="InstrumentHttpClient" /> to be <see langword="true"/> due to the underlying implementation.
    /// </summary>
    public bool InstrumentGrpcClient { get; }
}