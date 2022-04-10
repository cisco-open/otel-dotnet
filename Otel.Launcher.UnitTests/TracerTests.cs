using System;
using System.Net.Http;
using NUnit.Framework;
using Otel.Launcher.Tracing;

namespace Otel.Launcher.UnitTests;

public class TracerTests
{
    [SetUp]
    public void Setup()
    {
    }

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

            Trace.Init(options);

            new HttpClient().GetStringAsync("https://example.com").Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Assert.Fail(e.Message);
        }
    }
}