using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cisco.Opentelemetry.Specifications.Consts;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;
using OpenTelemetry;

namespace Cisco.Otel.Distribution.UnitTests;

public class TracerTests
{
    [Test]
    public void HappyPathTest()
    {
        var ciscoToken = "my-cisco-token";
        var exportedItems = new List<Activity>();

        var options = new CiscoOptions(
            ciscoToken,
            exporterOptions: new ExporterOptions[]
            {
                new ExporterOptions.InMemory(exportedItems),
                new ExporterOptions.Console()
            });

        var tracerProvider = Tracer.Init(options);

        var tracer = tracerProvider.GetTracer(options.ServiceName);

        using (tracer.StartActiveSpan("HappyPathSpan"))
        {
        }

        Assert.AreEqual(1, exportedItems.Count);
        Assert.AreEqual("HappyPathSpan", exportedItems.First().DisplayName);
    }

    [Test]
    public void ResourceSdkVersionTest()
    {
        var options = new CiscoOptions("my-cisco-token");

        var tracerProvider = Tracer.Init(options);

        var resource = tracerProvider.GetResource();

        var sdkVersion =
            resource
                .Attributes
                .First(a => a.Key == Consts.CISCO_SDK_VERSION)
                .Value;

        Assert.AreEqual("0.1.0", sdkVersion);
    }
}