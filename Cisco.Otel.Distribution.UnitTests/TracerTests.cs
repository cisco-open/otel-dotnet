using System;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;

namespace Cisco.Otel.Distribution.UnitTests;

public class TracerTests
{
    [Test]
    public void HappyPathTest()
    {
        try
        {
            var options = new CiscoOptions(
                new[]
                {
                    new ExporterOptions.Console()
                });

            var tracerProvider = Trace.Init(options);

            var tracer = tracerProvider.GetTracer(options.ServiceName);
            
            using var span = tracer.StartActiveSpan("HappyPathSpan");
            
            span.SetAttribute("test_attribute", 123);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Assert.Fail(e.Message);
        }
    }
}