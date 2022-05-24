using System.Reflection;
using OpenTelemetry.Resources;

namespace Cisco.Otel.Distribution.Tracing;

internal static class ResourceBuilderExtensions
{
    internal static ResourceBuilder AddCiscoVersion(this ResourceBuilder builder)
    {
        return builder
            .AddAttributes(new List<KeyValuePair<string, object>>
            {
                new("cisco.sdk.version", GetFileVersion()),
            });
    }
    
    private static string GetFileVersion()
    {
        var version = typeof(ResourceBuilderExtensions)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
        
        var hashIndex = version.IndexOf("+", StringComparison.Ordinal);
        
        return hashIndex > 0 
            ? version.Substring(0, hashIndex)
            : version;
    }
}