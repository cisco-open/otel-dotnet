using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;
using Trace = Cisco.Otel.Distribution.Tracing.Trace;

namespace Cisco.Otel.Distribution.UnitTests;

public class TracerTests
{
    [Test]
    public void HappyPathTest()
    {
        var exportedItems = new List<Activity>();
            
        var options = new CiscoOptions(
            new ExporterOptions[]
            {
                new ExporterOptions.InMemory(exportedItems),
                new ExporterOptions.Console()
            });

        var tracerProvider = Trace.Init(options);

        var tracer = tracerProvider.GetTracer(options.ServiceName);

        using (tracer.StartActiveSpan("HappyPathSpan"))
        {
        }

        Assert.AreEqual(1, exportedItems.Count);
        Assert.AreEqual("HappyPathSpan", exportedItems.First().DisplayName);
    }
}