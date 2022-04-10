using System;
using System.Collections.Generic;
using NUnit.Framework;
using Otel.Launcher.Tracing;

namespace Otel.Launcher.UnitTests;

public class OptionsTests
{
    [Test]
    public void OptionsDefaultsTest()
    {
        var exporterOptions = new List<ExporterOptions>
        {
            new ExporterOptions.Console()
        };

        var options = new CiscoOptions(exporterOptions);

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
                var otlpGrpc = new ExporterOptions.OtlpGrpc(null);
            },
            "Cisco Token cannot be null");

        Assert.Throws<ArgumentException>(() =>
            {
                var otlpHttp = new ExporterOptions.OtlpHttp(null);
            },
            "Cisco Token cannot be null");
    }
}