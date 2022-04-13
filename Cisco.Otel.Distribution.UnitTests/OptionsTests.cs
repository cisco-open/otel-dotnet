using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;

namespace Cisco.Otel.Distribution.UnitTests;

public class OptionsTests
{
    [Test]
    public void OptionsDefaultsTest()
    {
        var options = new CiscoOptions(new List<ExporterOptions>{ new ExporterOptions.Console()});

        Assert.AreEqual(Constants.DefaultServiceName, options.ServiceName);
    }

    [Test]
    public void EmptyExportersListTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var options = new CiscoOptions(new List<ExporterOptions>());
            },
            "Must contain one or more exporter options");
    }

    [Test]
    public void ExporterOptionsMissingTokenTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var otlpGrpc = new ExporterOptions.OtlpGrpc(string.Empty);
            },
            "Cisco Token cannot be null");

        Assert.Throws<ArgumentException>(() =>
            {
                var otlpHttp = new ExporterOptions.OtlpHttp(string.Empty);
            },
            "Cisco Token cannot be null");
    }
}