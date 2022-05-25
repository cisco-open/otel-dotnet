using System.Reflection;
using Cisco.Opentelemetry.Specifications.Consts;
using OpenTelemetry.Resources;

namespace Cisco.Otel.Distribution.Tracing;

internal static class ResourceBuilderExtensions
{
    internal static ResourceBuilder AddCiscoVersion(this ResourceBuilder builder)
    {
        return builder
            .AddAttributes(new List<KeyValuePair<string, object>>
            {
                new(Consts.CISCO_SDK_VERSION, GetSdkVersion()),
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