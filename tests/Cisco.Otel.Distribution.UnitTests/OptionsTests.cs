using System;
using System.Linq;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;
using Microsoft.Extensions.Configuration;

namespace Cisco.Otel.Distribution.UnitTests;

public class OptionsTests
{
    [Test]
    public void OptionsDefaultsTest()
    {
        var ciscoToken = "my-cisco-token";
        var options = new CiscoOptions(ciscoToken);

        Assert.AreEqual(Constants.DefaultServiceName, options.ServiceName);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpGrpc);
    }

    [Test]
    public void OptionsMissingTokenTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var emptyToken = new CiscoOptions("");
            },
            "Cisco Token cannot be null");

        Assert.Throws<ArgumentException>(() =>
            {
                var nullToken = new CiscoOptions(null!);
            },
            "Cisco Token cannot be null");
    }

    [Test]
    public void OptionsFromConfigurationFileTest()
    {
        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

        var options = CiscoOptionsHelper.FromConfiguration(configuration);

        Assert.AreEqual("Bearer my-cisco-token", options.CiscoToken);
        Assert.AreEqual("test-application", options.ServiceName);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpGrpc);

        var grpcExporter = (ExporterOptions.OtlpGrpc)options.ExporterOptions.First();

        Assert.AreEqual("http://localhost:4317", grpcExporter.CollectorEndpoint);
    }
}