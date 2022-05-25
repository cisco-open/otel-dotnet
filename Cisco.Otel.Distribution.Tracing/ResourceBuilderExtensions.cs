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
                new("cisco.sdk.version", GetSdkVersion()),
            });
    }

    private static string GetSdkVersion()
    {
        try
        {
            var version =
                Assembly
                    .GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

            return version;
        }
        catch (Exception)
        {
            return Constants.DefaultSdkVersion;
        }
    }
}